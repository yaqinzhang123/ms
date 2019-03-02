using System;
using System.Collections.Generic;
using System.Text;

namespace Mosaic.DTO
{
    public class ReadRateQRCode
    {
        public string ProductionLineName { get; set; }
        public int ProductionLineID { get; set; }
        public int CID { get; set; }
        public string MaterialNo { get; set; }
        public string Describe { get; set; }//物料描述
        public int  QRCodeCount { get; set; }
        public int TrueQRCodeCount { get; set; }
        public int FalseQRCodeCount { get; set; }
        public string ReadRate { get; set; }
        public string InstantReadRate { get; set; }//瞬时读码率
        public int RemainderTrue { get; set; }//真实尾数
        public int GroupCountTrue { get; set; }//真实兜数
        public bool RateFlag { get; set; }
        public bool RateFlag1 { get; set; }//瞬时报警标志
    }
}
