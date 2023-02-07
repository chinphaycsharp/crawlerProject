using CefSharp;
using CefSharp.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCCorp.CrawlerPreview.BUS;
using VCCorp.CrawlerPreview.Common;
using VCCorp.CrawlerPreview.DTO;

namespace VCCorp.CrawlerPreview
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser _browser = null;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cbNamePage.DisplayMember = "Text";
            cbNamePage.ValueMember = "Value";

            var items = new[] {
                    new { Text = "BestPrice Hotel", Value = NamePage.BESTPRICE_HOTEL },
                    new { Text = "BestPrice Tour", Value = NamePage.BESTPRICE_TOUR },
                    new { Text = "Airbnb", Value = NamePage.AIRBNB },
                    new { Text = "MyTour", Value = NamePage.MYTOUR },
                    new { Text = "VnTrip", Value = NamePage.VNTRIP },
                    new { Text = "Hotels.Com", Value = NamePage.ViHotels },
                    new { Text = "PasGo", Value = NamePage.PasGo}
            };
            cbNamePage.DataSource = items;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            rtbResult.Text = "";
            NamePage a = (NamePage)cbNamePage.SelectedValue;
            switch (a)
            {
                case NamePage.BESTPRICE_HOTEL:
                    await InitBrowser("https://www.bestprice.vn/");
                    await Task.Delay(5000);
                    await ContentCrawlerHotelBp();
                    await ContentCrawlerHotelBpDeatail();
                    while (true)
                    {
                        try
                        {
                            await Task.Delay(10_000);
                            ParserBestPriceHotel parserBestPrice = new ParserBestPriceHotel(_browser);
                            await parserBestPrice.GetListComment();
                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }

                case NamePage.BESTPRICE_TOUR:
                    await InitBrowser("https://www.bestprice.vn/");
                    await Task.Delay(5000);
                    await ContentCrawlerTourBp();
                    //Thread thrd1 = new Thread(new ThreadStart(ContentCrawlerTourBp));
                    //thrd1.Start();
                    await ContentCrawlerTourBpDeatail();
                    //Thread thrdDetails1 = new Thread(new ThreadStart(ContentCrawlerTourBpDeatail));
                    //thrdDetails1.Start();

                    while (true)
                    {
                        try
                        {
                            await Task.Delay(10_000);
                            ParserBestPriceTour parserBestPrice = new ParserBestPriceTour(_browser);
                            await parserBestPrice.GetListComment();
                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }

                case NamePage.AIRBNB:
                    await InitBrowser("https://www.airbnb.com.vn/");
                    while (true)
                    {
                        try
                        {
                            
                            await Task.Delay(10_000);
                            ParserAirbnb parser = new ParserAirbnb(_browser);
                            await parser.CrawlData();

                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }
                case NamePage.MYTOUR:
                    await InitBrowser("https://mytour.vn/");
                    while (true)
                    {
                        try
                        {
                          
                            await Task.Delay(10_000);
                            ParserMyTour parser = new ParserMyTour(_browser);
                            await parser.CrawlData();

                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }
                case NamePage.VNTRIP:
                    await InitBrowser("https://www.vntrip.vn/");
                    while (true)
                    {
                        try
                        {
                            
                            await Task.Delay(10_000);
                            ParserVnTrip parser = new ParserVnTrip(_browser);

                            await parser.CrawlData();

                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }
                case NamePage.ViHotels:
                    await InitBrowser("https://vi.hotels.com/"); 
                    while (true)
                    {
                        try
                        {
                            
                            await Task.Delay(10_000);
                            ParserHotels parser = new ParserHotels(_browser);

                            await parser.CrawlData();

                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }
                case NamePage.PasGo:
                    await InitBrowser("https://pasgo.vn/");
                    while (true)
                    {
                        try
                        {
                            
                            await Task.Delay(10_000);
                            ParserPasGo parser = new ParserPasGo(_browser);

                            await parser.CrawlData();

                        }
                        catch (Exception)
                        {
                        }
                        await Task.Delay(TimeSpan.FromHours(1));
                    }

            }
        }

        public async Task ContentCrawlerHotelBp()
        {
            WriteToBoxResult("---- Bắt đầu bóc tách tại chuyên mục: " + "https://www.bestprice.vn/");
            for (int i = 0; i < 100; i++)
            {
                string html = DownloadHtml.GetContentHtml("https://www.bestprice.vn/khach-san/trong-nuoc?page=" + i);

                //string html = DownloadHtml.GetContentHtml(txtUrl.Text);

                if (string.IsNullOrEmpty(html))
                {
                    WriteToBoxResult("---- ---- lỗi download Html, ex: " + DownloadHtml._error);
                    return;
                }
                else
                {
                    WriteToBoxResult("---- ---- download HTML tốt");
                }
                ParserBestPriceHotel parBus = new ParserBestPriceHotel();

                List<ContentDTO> lisArticle = await parBus.GetListContents(html);

                if (lisArticle == null || lisArticle.Count == 0)
                {
                    WriteToBoxResult("---- ---- lỗi bóc tách danh sách bài viết từ chuyên mục - vui lòng kiểm tra lại bộ parser");
                    return;
                }
                else
                {
                    WriteToBoxResult("---- ---- bóc thành công: " + lisArticle.Count.ToString() + " bài viết.");
                }

                foreach (ContentDTO art in lisArticle)
                {
                    WriteToBoxResult("---- ---- ---- " + art.Subject + " - " + "https://www.bestprice.vn/" + art.ReferUrl);
                }

            }
        }

        private async Task ContentCrawlerHotelBpDeatail()
        {
            //await Task.Delay(5000);
            ParserBestPriceHotel parBus = new ParserBestPriceHotel();
            List<ContentDTO> objArticle = await parBus.GetContentsDetail();
            if (objArticle == null)
            {
                WriteToBoxResult("---- ---- ---- ---- lỗi bóc tách nội dung bài viết- vui lòng kiểm tra lại bộ parser");
                return;
            }
        }

        private void WriteToBoxResult(string text)
        {
            rtbResult.AppendText(text + Environment.NewLine + Environment.NewLine);
        }

        public async Task InitBrowser(string urlBase)
        {
            if (_browser == null)
            {
                this.WindowState = FormWindowState.Maximized;
                CefSettings s = new CefSettings();

                Cef.Initialize(s);
                _browser = new ChromiumWebBrowser(urlBase);
                this.panel1.Controls.Add(_browser);
                _browser.Width = panel1.Width;
                _browser.Height = panel1.Height;
                _browser.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                _browser.Dock = DockStyle.Fill;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _browser.ShowDevTools();
        }

        private void rtbResult1_TextChanged(object sender, EventArgs e)
        {

        }
        public async Task ContentCrawlerTourBp()
        {
            WriteToBoxResult("---- Bắt đầu bóc tách tại chuyên mục: " + "https://www.bestprice.vn/");
            for (int i = 0; i < 5; i++)
            {
                string html = DownloadHtml.GetContentHtml("https://www.bestprice.vn/tour?page=" + i);

                //string html = DownloadHtml.GetContentHtml(txtUrl.Text);

                if (string.IsNullOrEmpty(html))
                {
                    WriteToBoxResult("---- ---- lỗi download Html, ex: " + DownloadHtml._error);
                    return;
                }
                else
                {
                    WriteToBoxResult("---- ---- download HTML tốt");
                }

                ParserBestPriceTour parBus = new ParserBestPriceTour();

                List<ContentDTO> lisArticle = await parBus.GetListContents(html);

                if (lisArticle == null || lisArticle.Count == 0)
                {
                    WriteToBoxResult("---- ---- lỗi bóc tách danh sách bài viết từ chuyên mục - vui lòng kiểm tra lại bộ parser");
                    return;
                }
                else
                {
                    WriteToBoxResult("---- ---- bóc thành công: " + lisArticle.Count.ToString() + " bài viết.");
                }

            }
        }

        private async Task ContentCrawlerTourBpDeatail()
        {
            ParserBestPriceTour parBus = new ParserBestPriceTour();
            List<ContentDTO> objArticle = await parBus.GetContentsDetail();

            if (objArticle == null)
            {
                WriteToBoxResult("---- ---- ---- ---- lỗi bóc tách nội dung bài viết- vui lòng kiểm tra lại bộ parser");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connetionString;
            MySqlConnection cnn;
            connetionString = "Server=127.0.0.1;Port=3306;Database=crawler_preview;Uid=root;Pwd=07081999;";
            cnn = new MySqlConnection(connetionString);
            cnn.Open();
            MessageBox.Show("Connection Open  !");
            cnn.Close();
        }
    }
}