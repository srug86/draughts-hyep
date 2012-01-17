using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    class Movement
    {
        private int srcRow;

        public int SrcRow
        {
            get { return srcRow; }
            set { srcRow = value; }
        }
        private int srcColumn;

        public int SrcColumn
        {
            get { return srcColumn; }
            set { srcColumn = value; }
        }
        private int dstRow;

        public int DstRow
        {
            get { return dstRow; }
            set { dstRow = value; }
        }
        private int dstColumn;

        public int DstColumn
        {
            get { return dstColumn; }
            set { dstColumn = value; }
        }
        private int value;

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Movement(int srcRow, int srcColumn, int dstRow, int dstColumn)
        {
            this.srcRow = srcRow;
            this.srcColumn = srcColumn;
            this.dstRow = dstRow;
            this.dstColumn = dstColumn;
            this.value = 0;
        }
    }
}
