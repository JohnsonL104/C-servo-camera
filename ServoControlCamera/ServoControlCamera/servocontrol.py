import serial
import struct
import sys
'''data.write(struct.pack('>B', pos))'''
data = serial.Serial('com3', 9600, timeout = 1)
	


def p1():
	pos = int(sys.argv[1])
	data.write(struct.pack('>B', pos))

		
		
while True:
	p1()

	