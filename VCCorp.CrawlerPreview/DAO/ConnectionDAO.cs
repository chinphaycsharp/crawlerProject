﻿namespace VCCorp.CrawlerPreview.DAO
{
    public class ConnectionDAO
    {
        /* Server */
        //public const string ConnectionToTableLinkProduct = @"Data Source=quangle,3306;Initial Catalog = crawler_preview; User ID = root; Password=123456";
        public const string ConnectionToTableLinkProduct = @"Server=127.0.0.1;Port=3306;Database=crawler_preview;Uid=root;Pwd=07081999;";
        //public const string ConnectionToTableReportDaily = @"Server=localhost;User ID = root;Password=123456aA@;";

        /* Local */
        //public const string ConnectionToTableLinkProduct = @"Server=192.168.23.22;User ID = minhdq;Password=wgy2FdMt0rXfcmCWGSqa;";

        public const string KEY_BOT = "5510917559:AAFc29FuT3isv31rJJ-Hf3U8J3TQm7wUjn8";
        public static readonly long ID_TELEGRAM_BOT_GROUP_COMMENT_ECO = 966893697;

    }
}
