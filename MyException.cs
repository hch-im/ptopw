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

namespace ptopw
{
    public class MyException : Exception
    {
        public string s;
        public int error;

        public const int MSG_PARSE_ERROR = 1;//parse config file
        public const int MSG_CREATE_COUNTER_ERROR = 2;//create performance counter
        public const int MSG_UPDATE_COUNTER_ERROR = 3;//update performance counter
        public const int MSG_READ_MESSAGE_ERROR = 4;//read message
        public const int MSG_WRITE_MESSAGE_ERROR = 5;//write message
        public const int MSG_CREATE_APISERVER_ERROR = 6;//create api server
        public const int MSG_INIT_SYSTEMINFO_ERROR = 7;//init system info
        public const int MSG_INIT_STAT_COUNTER_ERROR = 8;//init counter
        public const int MSG_INIT_LOG_INUSE_ERROR = 9;//init counter

        public MyException(int errorType):base()  
        {
            error = errorType;
            s=null;  
        }
  
        public MyException(string message, int errorType):base()  
        {  
            s=message.ToString();
            error = errorType;
        }
    }
}
