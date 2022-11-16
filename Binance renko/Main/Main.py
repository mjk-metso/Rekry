import websocket
import _thread
import time
import rel
import json
import math
import requests
import numpy as np
from statistics import mode
import matplotlib.pyplot as plt
import pyformulas as pf
from matplotlib.colors import LinearSegmentedColormap, Normalize
from datetime import datetime
import pandas as pd
from alive_progress import alive_bar
from playsound import playsound

WEBSOCKET = 'wss://fstream.binance.com/ws/ethbusd@aggTrade'
BARS = 500
DATAFRAME = []
TIME = []
OPEN = []
CLOSE = []
HIGH = []
LOW = []
boxsize = 1
color = 'g-'

LAST_UPDATE = 0

def axes_with_pixels(width, height):
    px = 1/plt.rcParams['figure.dpi']
    fig_width, fig_height = np.array([width, height])
    fig, ax = plt.subplots(figsize=(fig_width*px, fig_height*px))
    return fig, ax

plt.style.use('dark_background')
fig, ax = axes_with_pixels(1920, 1080)
screen = pf.screen(title='ETHBUSD renko')

def candlestick(t, o, h, l, c):
    color = ["green" if close_price > open_price else "blue" for close_price, open_price in zip(c, o)]
    plt.bar(x=t, height=np.abs(o-c), bottom=np.min((o,c), axis=0), width=0.9, color=color)

def average(lst):
    return sum(lst) / len(lst)

def update(price, T):
    global CLOSE, TIME, OPEN, LOW, HIGH, DATAFRAME, BARS, color
    plt.cla()
    ax.cla()
    
    plt.ylim(min(CLOSE)-3, max(CLOSE)+3)
    if len(CLOSE) > BARS:
        CLOSE = CLOSE[-BARS:]
        TIME = TIME[-BARS:]
        DATAFRAME = DATAFRAME[-BARS:]
        OPEN = OPEN[-BARS:]
        HIGH = HIGH[-BARS:]
        LOW = LOW[-BARS:]
    #plt.plot(TIME, CLOSE, color)
    df = pd.DataFrame(DATAFRAME, columns='time open high low close'.split())
    df.set_index('close')
    candlestick(df.index, df.open, df.high, df.low, df.close)
    
    ax.set_title('ETHBUSD')
    ax.set_xticks(df.index[::50])
    ax.set_xticklabels(df.time[::50], rotation=30)
    ax.set_xlabel('Time')
    ax.set_ylabel('Price')

    plt.axhline(y = price, color = 'r', linestyle = ':')
    plt.annotate('%0.2f' % price, xy=(1, price), xytext=(8, 0), 
                 xycoords=('axes fraction', 'data'), textcoords='offset points')
    plt.annotate(T.strftime("%H:%M:%S"), xy=(1, price-2), xytext=(8, 0), 
                 xycoords=('axes fraction', 'data'), textcoords='offset points')

    plt.grid(alpha=0.2)
    plt.tight_layout()

    fig.canvas.draw()
    image = np.frombuffer(fig.canvas.tostring_rgb(), dtype=np.uint8)
    image = image.reshape(fig.canvas.get_width_height()[::-1] + (3,))
    screen.update(image)

def on_message(ws, message):
    global CLOSE, TIME, OPEN, LOW, HIGH, DATAFRAME, BARS, color, LAST_UPDATE
    msg = json.loads(message)
    
    price = float(msg['p'])

    if price == 0:
        return

    try:
        raw_time = msg['E']/1000.0
        time_ = datetime.fromtimestamp(raw_time)
    except:
        raw_time = msg['T']/1000.0
        time_ = datetime.fromtimestamp(raw_time)

    if len(CLOSE) == 0:
        CLOSE.append(round(price))
        return

    if len(CLOSE) == 1:
        if price >= CLOSE[-1] + boxsize or price <= CLOSE[-1] - boxsize:
            CLOSE.append(round(price))
        return

    upBar = price >= CLOSE[-1] + boxsize if CLOSE[-1] > CLOSE[-2] else price >= CLOSE[-1] + boxsize * 2
    downBar = price <= CLOSE[-1] - boxsize if CLOSE[-1] < CLOSE[-2] else price <= CLOSE[-1] - boxsize * 2

    if upBar or downBar:
        if upBar and ws != None:
            playsound('./Updoot.mp3', block=False)
        if downBar and ws != None:
            playsound('./Downdoot.mp3', block=False)
        
        if abs(round(price) - CLOSE[-1]) == 2:
            if downBar:
                OPEN.append(CLOSE[-1] - boxsize)
            if upBar:
                OPEN.append(CLOSE[-1] + boxsize)
        else:
            OPEN.append(CLOSE[-1])
        CLOSE.append(round(price))
        HIGH.append(max([CLOSE[-1], OPEN[-1]]))
        LOW.append(min([CLOSE[-1], OPEN[-1]]))
        TIME.append(time_)
        DATAFRAME.append([TIME[-1], OPEN[-1], HIGH[-1], LOW[-1], CLOSE[-1]])

    if ws is not None: # and raw_time > LAST_UPDATE + 1:
        update(price, time_)
        # LAST_UPDATE = raw_time


def on_error(ws, error):
    print(error)

def on_close(ws, close_status_code, close_msg):
    print("### closed ###")

def on_open(ws):
    pass

def fetch_hist():
    date = datetime(2022, 11, 15) - datetime(1970, 1, 1)
    date = round(date.total_seconds()*1000)
    ten_min = 10*60*1000

    datenow = datetime.utcnow() - datetime(1970, 1, 1)
    datenow = round(datenow.total_seconds()*1000)

    target = (datenow - date)/1000/60
    with alive_bar(manual=True, title='Fetching history') as bar:
        FROM = date
        while True:
            datenow = datetime.utcnow() - datetime(1970, 1, 1)
            datenow = round(datenow.total_seconds()*1000)
            TO = FROM + ten_min

            if TO > datenow:
                TO = datenow

            HISTORICAL = 'https://api.binance.com/api/v3/aggTrades?symbol=ETHBUSD&startTime={}&endTime={}&limit=1000'.format(FROM, TO)

            hist = requests.get(HISTORICAL)
            for line in hist.json():
                on_message(None, json.dumps(line))
                FROM = line['T']

            time.sleep(0.2)
            progress = (datenow - FROM)/1000/60
            status = 1 - progress/target
            bar(status)
            if hist.status_code != 200:
                print(hist.text)
            if datenow - FROM < 10000:
                break

def main():
    fetch_hist()
    print('LIVE')

    ws = websocket.WebSocketApp(WEBSOCKET,
                              on_open=on_open,
                              on_message=on_message,
                              on_error=on_error,
                              on_close=on_close)


    ws.run_forever(dispatcher=rel, reconnect=5)  # Set dispatcher to automatic reconnection
    rel.signal(2, rel.abort)  # Keyboard Interrupt
    rel.dispatch()



if __name__ == "__main__":
    main()