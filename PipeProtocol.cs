/*
 *   Copyright (C) 2011, Mobile and Internet Systems Laboratory.
 *   Department of Computer Science, Wayne State University.
 *   All rights reserved.
 *
 *   Authors: Hui Chen (huichen@wayne.edu), Youhuizi Li (huizi@wayne.edu) 
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace ptopw
{
    public class PipeProtocol
    {
        private Stream ioStream;

        public PipeProtocol(Stream io)
        {
            ioStream = io;
        }

        public Message ReadMessage()
        {
            try
            {
                int len = 0;
                byte[] lenbyte = new byte[4];
                ioStream.Read(lenbyte, 0, 4);
                len = BitConverter.ToInt32(lenbyte, 0);

                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);

                return Message.parse(inBuffer);
            }
            catch (System.Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_READ_MESSAGE_ERROR);
            }
        }

        public int WriteMessage(Message msg)
        {
            try{
                byte[] outBuffer = msg.encode();
                int len = outBuffer.Length;
                if (len > Int32.MaxValue)
                {
                    len = Int32.MaxValue;
                }
            
                ioStream.Write(BitConverter.GetBytes(len), 0, 4);
                ioStream.Write(outBuffer, 0, len);
                ioStream.Flush();

                return outBuffer.Length + 4;
            }
            catch (System.Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_WRITE_MESSAGE_ERROR);
            }
        }
    }

    public class Message
    {
        public const int GAP_VALUE = 28;

        public const int MSG_REQ_COMPONENT = 1;
        public const int MSG_REQ_PROCESS = 2;
        public const int MSG_REQ_SYSTEM_ENG = 3;
        public const int MSG_REQ_CPU_ENG = 4;
        public const int MSG_REQ_MEM_ENG = 5;
        public const int MSG_REQ_DISK_ENG = 6;
        public const int MSG_REQ_NIC_ENG = 7;
        public const int MSG_REQ_PRO_ENG = 8;
        public const int MSG_REQ_PRO_CPU_ENG = 9;
        public const int MSG_REQ_PRO_MEM_ENG = 10;
        public const int MSG_REQ_PRO_DISK_ENG = 11;
        public const int MSG_REQ_PRO_NIC_ENG = 12;
        public const int MSG_REQ_PROINFO = 13;
        public const int MSG_REQ_PRO_AVGPOWER = 14;
        public const int MSG_REQ_SYS_AVGPOWER = 15;

        public const int MSG_RESP_COMPONENT = 29;
        public const int MSG_RESP_PROCESS = 30;
        public const int MSG_RESP_SYSTEM_ENG = 31;
        public const int MSG_RESP_CPU_ENG = 32;
        public const int MSG_RESP_MEM_ENG = 33;
        public const int MSG_RESP_DISK_ENG = 34;
        public const int MSG_RESP_NIC_ENG = 35;
        public const int MSG_RESP_PRO_ENG = 36;
        public const int MSG_RESP_PRO_CPU_ENG = 37;
        public const int MSG_RESP_PRO_MEM_ENG = 38;
        public const int MSG_RESP_PRO_DISK_ENG = 39;
        public const int MSG_RESP_PRO_NIC_ENG = 40;
        public const int MSG_RESP_PROINFO = 41;
        public const int MSG_RESP_PRO_AVGPOWER = 42;
        public const int MSG_RESP_SYS_AVGPOWER = 43;

        public int type;
        public int pid;
        public int flag;
        public long time;//ms
        public double energy;
        public double[] data;
        public Hashtable info;
       // public double[,] pinfo;

        public Message(int t)
        {
            type = t;
        }

        public static Message parse(byte[] buffer)
        {
            int i = 0;
            int type = buffer[i++];

            Message msg = new Message(type);
            switch (type)
            {
                case Message.MSG_REQ_COMPONENT://flag
                    i = msg.decodeInt(buffer, i, ref msg.flag);
                    break;
                case Message.MSG_REQ_PROCESS://flag and pid
                    i = msg.decodeInt(buffer, i, ref msg.flag);
                    i = msg.decodeInt(buffer, i, ref msg.pid);
                    break;
                case Message.MSG_REQ_SYSTEM_ENG://time
                case Message.MSG_REQ_CPU_ENG:
                case Message.MSG_REQ_MEM_ENG:
                case Message.MSG_REQ_DISK_ENG:
                case Message.MSG_REQ_NIC_ENG:
                    i = msg.decodeLong(buffer, i, ref msg.time);
                    break;
                case Message.MSG_REQ_PRO_ENG://time and pid
                case Message.MSG_REQ_PRO_CPU_ENG:
                case Message.MSG_REQ_PRO_MEM_ENG:
                case Message.MSG_REQ_PRO_DISK_ENG:
                case Message.MSG_REQ_PRO_NIC_ENG:
                    i = msg.decodeLong(buffer, i, ref msg.time);
                    i = msg.decodeInt(buffer, i, ref msg.pid);
                    break;
                case Message.MSG_RESP_COMPONENT://data
                case Message.MSG_RESP_PROCESS:
                    i = msg.decodeDoubleArray(buffer, i, ref msg.data);
                    break;
                case Message.MSG_RESP_PROINFO:// process info
                    i = msg.decodeTable(buffer, i, ref msg.info);
                    break;
                case Message.MSG_RESP_SYSTEM_ENG://energy
                case Message.MSG_RESP_CPU_ENG:
                case Message.MSG_RESP_MEM_ENG:
                case Message.MSG_RESP_DISK_ENG:
                case Message.MSG_RESP_NIC_ENG:
                case Message.MSG_RESP_PRO_ENG:
                case Message.MSG_RESP_PRO_CPU_ENG:
                case Message.MSG_RESP_PRO_MEM_ENG:
                case Message.MSG_RESP_PRO_DISK_ENG:
                case Message.MSG_RESP_PRO_NIC_ENG:
                    i = msg.decodeDouble(buffer, i, ref msg.energy);
                    break;
                case Message.MSG_RESP_PRO_AVGPOWER:
                    i = msg.decodeAvgPower(buffer, i, ref msg.info);
                    break;
                case Message.MSG_RESP_SYS_AVGPOWER:
                    i = msg.decodeDouble(buffer, i, ref msg.energy);
                    break;
                default:
                    break;
            }
            return msg;
        }

        public byte[] encode()
        {
            int i = 0;
            byte[] buffer = new byte[10240];
            buffer[i++] = (byte)(type & 255);

            switch (type)
            {
                case Message.MSG_REQ_COMPONENT://flag
                    i = encodeInt(buffer, i, flag);
                    break;
                case Message.MSG_REQ_PROCESS://flag and pid
                    i = encodeInt(buffer, i, flag);
                    i = encodeInt(buffer, i, pid);
                    break;
                case Message.MSG_REQ_SYSTEM_ENG://time
                case Message.MSG_REQ_CPU_ENG:
                case Message.MSG_REQ_MEM_ENG:
                case Message.MSG_REQ_DISK_ENG:
                case Message.MSG_REQ_NIC_ENG:
                    i = encodeLong(buffer, i, time);
                    break;
                case Message.MSG_REQ_PRO_ENG://time and pid
                case Message.MSG_REQ_PRO_CPU_ENG:
                case Message.MSG_REQ_PRO_MEM_ENG:
                case Message.MSG_REQ_PRO_DISK_ENG:
                case Message.MSG_REQ_PRO_NIC_ENG:
                    i = encodeLong(buffer, i, time);
                    i = encodeInt(buffer, i, pid);
                    break;                                          // above encode requests, below encode response, process energy table has no parameter
                case Message.MSG_RESP_COMPONENT://data
                case Message.MSG_RESP_PROCESS:
                    i = encodeDoubleArray(buffer, i, data);
                    break;
                case Message.MSG_RESP_PROINFO:
                    i = encodeTable(buffer, i, info);
                    break;
                case Message.MSG_RESP_SYSTEM_ENG://energy
                case Message.MSG_RESP_CPU_ENG:
                case Message.MSG_RESP_MEM_ENG:
                case Message.MSG_RESP_DISK_ENG:
                case Message.MSG_RESP_NIC_ENG:
                case Message.MSG_RESP_PRO_ENG:
                case Message.MSG_RESP_PRO_CPU_ENG:
                case Message.MSG_RESP_PRO_MEM_ENG:
                case Message.MSG_RESP_PRO_DISK_ENG:
                case Message.MSG_RESP_PRO_NIC_ENG:
                    i = encodeDouble(buffer, i, energy);
                    break;
                case Message.MSG_RESP_PRO_AVGPOWER:
                    i = encodeAvgPower(buffer, i, info);
                    break;
                case Message.MSG_RESP_SYS_AVGPOWER:
                    i = encodeDouble(buffer, i, energy);
                    break;                
                default:
                    break;
            }

            byte[] res = new byte[i];
            Array.Copy(buffer, res, i);
            return res;
        }

        public Message opossiteMessage()
        {
            if (type > GAP_VALUE)
                return new Message(type - GAP_VALUE);
            else
                return new Message(type + GAP_VALUE);
        }

        private int encodeInt(byte[] data, int index, int number)
        {
            byte[] temp = BitConverter.GetBytes(number);
            Array.Copy(temp, 0, data, index, temp.Length);
            return index + temp.Length;
        }

        private int encodeLong(byte[] data, int index, long number)
        {
            byte[] temp = BitConverter.GetBytes(number);
            Array.Copy(temp, 0, data, index, temp.Length);
            return index + temp.Length;
        }

        private int encodeDouble(byte[] data, int index, double number)
        {
            byte[] temp = BitConverter.GetBytes(number);
            Array.Copy(temp, 0, data, index, temp.Length);
            return index + temp.Length;
        }

        private int encodeDoubleArray(byte[] data, int index, double[] number)
        {
            if(number == null)
                index = encodeInt(data, index, 0);
            else
                index = encodeInt(data, index, number.Length);

            for(int i = 0; i < number.Length; i++)
            {
                byte[] temp = BitConverter.GetBytes(number[i]);
                Array.Copy(temp, 0, data, index, temp.Length);
                index += temp.Length;
            }


            return index;
        }

        private int encodeTable(byte[] data, int index, Hashtable info)
        {
            if (info.Count == 0)
                index = encodeInt(data, index, 0);
            else
                index = encodeInt(data, index, info.Count);

            foreach (ProcessEnergy pe in info.Values)
            {
                if (pe.no != 0)
                {
                   byte[] temp = BitConverter.GetBytes(pe.no);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;

                    temp = BitConverter.GetBytes(pe.cpuPower);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;

                    temp = BitConverter.GetBytes(pe.memPower);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;

                    temp = BitConverter.GetBytes(pe.diskPower);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;

                    temp = BitConverter.GetBytes(pe.nicPower);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;
                }
            }

            return index;
 
        }

        private int encodeAvgPower(byte[] data, int index, Hashtable info)
        {
            if (info.Count == 0)
                index = encodeInt(data, index, 0);
            else
                index = encodeInt(data, index, info.Count);

            foreach (ProcessEnergy pe in info.Values)
            {

                    byte[] temp = BitConverter.GetBytes(pe.no);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;

                    temp = BitConverter.GetBytes(pe.weightedPower);
                    Array.Copy(temp, 0, data, index, temp.Length);
                    index += temp.Length;                
            }

            return index;
        }

        private int decodeInt(byte[] data, int index, ref int number)
        {
            number = BitConverter.ToInt32(data, index);
            return index + 4;
        }

        private int decodeLong(byte[] data, int index, ref long number)
        {
            number = BitConverter.ToInt64(data, index);
            return index + 8;
        } 

        private int decodeDouble(byte[] data, int index, ref double number)
        {
            number = BitConverter.ToDouble(data, index);
            return index + 8;
        }

        private int decodeDoubleArray(byte[] data, int index, ref double[] number)
        {
            int leng = BitConverter.ToInt32(data, index);
            index += 4;
            if (leng != 0)
                number = new double[leng];
            else
                number = null;

            for (int i = 0; i < leng; i++)
            {
                number[i] = BitConverter.ToDouble(data, index);
                index += 8;
            }

            return index;
        }

        private int decodeTable(byte[] data, int index, ref Hashtable info)
        {
            int leng = BitConverter.ToInt32(data, index);
            index += 4;
            if (leng != 0)
                info = new Hashtable();
                //info = new double[leng,5];
            else
                info = null;

            for (int i = 0; i < leng; i++)
            {
                      int k = BitConverter.ToInt32(data, index);
                      index += 4;
                      double[] pinfo = new double[5];
                    
                      for (int g = 0; g < 4; g++)
                      {
                          pinfo[g] = BitConverter.ToDouble(data, index);
                          index += 8;
                      }
                 
                   if(k!=0)
                    info.Add(k, pinfo);
               
            }

            return index;
        }

        private int decodeAvgPower(byte[] data, int index, ref Hashtable info)
        {
            int leng = BitConverter.ToInt32(data, index);
            index += 4;
            if (leng != 0)
                info = new Hashtable();
            else
                info = null;

            for (int i = 0; i < leng; i++)
            {
                int k = BitConverter.ToInt32(data, index);
                index += 4;
                double wpower = BitConverter.ToDouble(data, index);
                index += 8;

                info.Add(k, wpower);      
            }

            return index;
        }
    }
}
