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
using System.Collections;
using System.IO;

namespace ptopw
{
    class ConfigFileParser
    {
        public static String SPACES = " \t\r\n";

	    string filename;
        public ConfigFileParser(string file)
        {
            filename = file;
        }

        public void parse(Hashtable param)
        {
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                sr = new StreamReader(fs);
                string str;

                while ((str = sr.ReadLine()) != null)
                {
                    parseStr(param, str);
                }

            }catch(Exception ex){
                throw new MyException(ex.Message, MyException.MSG_PARSE_ERROR);
            }
            finally{
                if(sr != null)sr.Close();
                if(fs != null)fs.Close();
            }
        }

	    private void parseStr(Hashtable param, string str)
        {
	        if(str.Equals(""))
		        return;

	        str = str.Trim();
	        if(str[0] == '#')
		        return;

	        int ind = str.IndexOf("#");
	        if(ind > 0)
		        str = str.Substring(0, ind);
            String[] strs = MySplit(str, ' ');
	        
	        if(strs.Length >= 2)
		        param.Add(strs[0], strs[1]);
        }

        private string[] MySplit(string str, char delim)
        {
            int cutAt;
            String[] strs = new String[2];
            cutAt = str.IndexOf(delim);
            if (cutAt != -1)
            {
                strs[0] = str.Substring(0, cutAt);
                strs[1] = str.Substring(cutAt + 1).Trim();
            }
            return strs;
        }
    }
}
