import serial
import struct
import sys

data = serial.Serial('com3', 9600, timeout = 1)
pos = int(sys.argv[1]])

data.write(struct.pack('>B', pos))