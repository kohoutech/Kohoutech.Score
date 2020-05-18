/* ----------------------------------------------------------------------------
Kohoutech Score Library
Copyright (C) 1997-2020  George E Greaney

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Kohoutech.Score;

namespace Kohoutech.Score.Symbols
{
    public class Symbol
    {
        public Beat beat;
        public Staff staff;

        public float left;
        public float top;
        public float xpos;                
        public float ypos;                

        public Symbol()
        {
            beat = null;
            staff = null;
            left = 0;
            top = 0;
            xpos = 0;
            ypos = 0;
        }

        public virtual void setBeat(Beat _beat)
        {
            beat = _beat;
            staff = beat.measure.staff;
        }

        public virtual void layout()
        {
        }

        public virtual void setPos(float _xpos, float _ypos)
        {
            xpos = left + _xpos;
            ypos = top + _ypos;
        }

        public virtual void dump()
        {
        }

        public virtual void paint(Graphics g)
        {
        }

//- common symbols ------------------------------------------------------------

        //public static void drawSharp(Graphics g, float xpos, float ypos)
        //{
        //}

        //public static void drawFlat(Graphics g, float xpos, float ypos)
        //{
        //}

    }

//-----------------------------------------------------------------------------

    public class BarLine : Symbol
    {
        public BarLine()
        {
        }
    }
    
//-----------------------------------------------------------------------------

    //public class Rest : Symbol
    //{
    //    public Rest(int _start, int _len) : base()
    //    {
    //        beat = _start;
    //        len = _len;
    //    }

    //}

}
