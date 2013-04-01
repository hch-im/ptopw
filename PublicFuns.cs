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
    public class PublicFuns
    {
        public static Int32 str2Int32(String str, Int32 defaultVal)
        {
            Int32 value;
            try{
                value = Convert.ToInt32(str);
                return value;
            }catch(Exception ex){
                return defaultVal;
            }

        }

        public static double str2Double(String str, double defaultVal)
        {
            double value;
            try
            {
                value = Convert.ToDouble(str);
                return value;
            }
            catch (Exception ex)
            {
                return defaultVal;
            }

        }
    }
}
