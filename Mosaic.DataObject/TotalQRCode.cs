using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class TotalQRCode
    {
        public int ProductionLineID { get; set; }
        public string ProductionLineName { get; set; }
        public int CID { get; set; }
        public string MaterialNo { get; set; }
        public int Total { get; set; }
        public int Remainder { get {
                return Total % 50;
            } }//余数
        public int GroupCount { get
            {
                return Total / 50;
            } }
        public int RemainderTrue { get; set; }//真实尾数
        public int GroupCountTrue { get; set; }//真实兜数
        public string Describe { get; set; }//描述
    }
}
