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
using System.Xml;

using Kohoutech.Score.MusicXML;
using Kohoutech.Score.Symbols;

namespace Kohoutech.Score
{
    public class ScoreDoc
    {
        public ScoreSheet sheet;

        public String filename;

        //scoreheader
        //public Work work;
        //public String movementNumber;
        //public String movementTitle;
        //public Identification identification;
        //public Defaults defaults;
        //public List<Credit> credits;
        //public PartList partList;

        public List<Part> parts;
        public Part curPart;

        public int curMeasure;
        public decimal curBeat;
        public float curStaffPos;

        //public float docWidth;
        //public float docHeight;

        //global settings
        public float staffMargin;
        public float staffSpacing;
        public float staffHeight;

//- cons ----------------------------------------------------------------------

        public ScoreDoc()
        {
            //scoreheader
            //work = null;
            //movementNumber = null;
            //movementTitle = null;
            //identification = null;
            //defaults = null;
            //credits = new List<Credit>();
            //partList = null;

            //docHeight = sheet.Height;
            //docWidth = sheet.Width;

            //globals
            staffMargin = 50;
            staffSpacing = 8;
            staffHeight = staffSpacing * 5;

            parts = new List<Part>();
            curPart = null;
            curMeasure = 0;
            curStaffPos = 0;
        }

        public void dump()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                Console.WriteLine("part [" + (i+1) + "]");
                parts[i].dump();
            }
        }

        public void resize(int width, int height)
        {
            //docWidth = width;
            //docHeight = height;
            //if (curPart != null)
            //{
            //    curPart.resize(width, height);
            //}
        }

//- painting ------------------------------------------------------------------

        public void paint(Graphics g)
        {
            if (curPart != null)
            {
                curPart.paint(g);
            }
        }
    }    
}

//Console.WriteLine("there's no sun in the shadow of the wizard");
