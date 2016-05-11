using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace VisualFormTest
{
    //画像処理のロジック
    public static class ImageEdit
    {
        //黒塗りの開始地点（100％の場合）
        const int PortFillX = 100;
        const int PortFillY = 5;
        const int PortFillWidth = 150;
        const int PortFillHeight = 20;
        //戦闘用
        const int BattleFillX = 60;
        const int BattleFillY = 82;
        const int BattleFillWidth = 150;
        const int BattleFillHeight = 20;
        //改装用
        const int FleetTrimX = 310;
        const int FleetTrimY = 95;
        const int FleetTrimWidth = 490;
        const int FleetTrimHeight = 375;
        const int FleetTrimColNum = 2;
        //母港編成用
        const int PortTrimX = 108;
        const int PortTrimY = 130;
        const int PortTrimWidth = 690;
        const int PortTrimHeight = 335;

        //母港用の黒塗り
        public static async Task FillBlackPortAsync(string[] pictures)
        {
            if (pictures.Length == 0) throw new ArgumentNullException();
            await Task.Factory.StartNew(() =>
                {
                    foreach (string x in pictures)
                    {
                        //ファイルの読み込み
                        using (Bitmap canvas = new Bitmap(x))
                        {
                            //ズームの倍率を求める
                            double zoom = (double)canvas.Width / 800;
                            if (zoom <= 0.0) return;
                            //塗りつぶす領域
                            Rectangle rect = new Rectangle((int)(PortFillX * zoom), (int)(PortFillY * zoom), (int)(PortFillWidth * zoom), (int)(PortFillHeight * zoom));

                            //塗りつぶし
                            using (Graphics g = Graphics.FromImage(canvas))
                            {
                                g.FillRectangle(Brushes.Black, rect);
                            }

                            //出力のファイル名
                            string extension = Path.GetExtension(x);
                            string outfilename = string.Format("{0}_censored{1}", x.Replace(extension, ""), ".png");
                            //保存
                            canvas.Save(outfilename);
                        }
                    }
                });
        }

        //戦闘用の黒塗り
        public static async Task FillBlackBattleAsync(string[] pictures)
        {
            if (pictures.Length == 0) throw new ArgumentNullException();
            await Task.Factory.StartNew(() =>
            {
                foreach (string x in pictures)
                {
                    //ファイルの読み込み
                    using (Bitmap canvas = new Bitmap(x))
                    {
                        //ズームの倍率を求める
                        double zoom = (double)canvas.Width / 800;
                        if (zoom <= 0.0) return;
                        //塗りつぶす領域
                        Rectangle rect = new Rectangle((int)(BattleFillX * zoom), (int)(BattleFillY * zoom), (int)(BattleFillWidth * zoom), (int)(BattleFillHeight * zoom));

                        //塗りつぶし
                        using (Graphics g = Graphics.FromImage(canvas))
                        {
                            g.FillRectangle(Brushes.Black, rect);
                        }

                        //出力のファイル名
                        string extension = Path.GetExtension(x);
                        string outfilename = string.Format("{0}_censored{1}", x.Replace(extension, ""), ".png");
                        //保存
                        canvas.Save(outfilename);
                    }
                }
            });
        }

        //母港編成用のトリミング
        public static async Task TrimPortFleetAsync(string[] pictures)
        {
            if (pictures.Length == 0) throw new ArgumentNullException();
            await Task.Factory.StartNew(() =>
            {
                foreach (string x in pictures)
                {
                    //ファイルの読み込み
                    using (Bitmap canvas = new Bitmap(x))
                    {
                        //ズームの倍率を求める
                        double zoom = (double)canvas.Width / 800;
                        if (zoom <= 0.0) return;
                        //切り抜く領域を求める
                        Rectangle rect = new Rectangle(
                                (int)((double)PortTrimX * zoom), (int)((double)PortTrimY * zoom),
                                (int)((double)PortTrimWidth * zoom), (int)((double)PortTrimHeight * zoom)
                            );
                        //トリミング
                        using(Bitmap trimming = canvas.Clone(rect, canvas.PixelFormat))
                        {
                            //出力のファイル名
                            string extension = Path.GetExtension(x);
                            string outfilename = string.Format("{0}_trimmed{1}", x.Replace(extension, ""), ".png");
                            //保存
                            trimming.Save(outfilename);
                        }
                    }
                }
            });
        }

        //画像の結合
        public static async Task CombineAsync(string[] pictures, int colnum)
        {
            if (pictures.Length == 0) throw new ArgumentNullException();
            await Task.Factory.StartNew(() =>
            {
                //最初の1枚のサイズを取得
                int basewidth = 0;
                int baseheight = 0;
                using (Bitmap first = new Bitmap(pictures[0]))
                {
                    basewidth = first.Width;
                    baseheight = first.Height;
                }

                //サイズの決定
                int combinewidth = basewidth * colnum;
                int combineheight = baseheight * (((pictures.Length - 1) / colnum) + 1);
                if(pictures.Length < colnum)
                {
                    combinewidth = basewidth * pictures.Length;
                }

                using (Bitmap result = new Bitmap(combinewidth, combineheight))
                {
                    using (Graphics g = Graphics.FromImage(result))
                    {
                        //全体を黒で塗りつぶし
                        g.FillRectangle(Brushes.Black, new Rectangle(0, 0, result.Width, result.Height));

                        //個別に貼り付け
                        foreach (int i in Enumerable.Range(0, pictures.Length))
                        {
                            using (Bitmap paste = new Bitmap(pictures[i]))
                            {
                                //コピーする領域の決定（サイズ以上だとOut of Memory例外が発生）
                                int copywidth, copyheight;
                                if (paste.Width >= basewidth) copywidth = basewidth;
                                else copywidth = paste.Width;
                                if (paste.Height >= baseheight) copyheight = baseheight;
                                else copyheight = paste.Height;
                                Rectangle srcrect = new Rectangle(0, 0, copywidth, copyheight);

                                //コピー先の領域
                                int x = i % colnum;
                                int y = i / colnum;
                                Rectangle destrect = new Rectangle(x * basewidth, y * baseheight, basewidth, baseheight);

                                //コピー
                                g.DrawImage(paste, destrect, srcrect, GraphicsUnit.Pixel);
                            }
                        }
                    }
                    //出力のファイル名
                    string extension = Path.GetExtension(pictures[0]);
                    string outfilename = string.Format("{0}_combined{1}", pictures[0].Replace(extension, ""), ".png");
                    //保存
                    result.Save(outfilename);
                }
            });
        }

        //編成用にトリミングして結合
        public static async Task FleetTrimCombineAsync(string[] pictures)
        {
            if (pictures.Length == 0) throw new ArgumentNullException();
            await Task.Factory.StartNew(() =>
                {
                    //最初の1枚から倍率の取得
                    double zoom = 0;
                    int basewidth = 0;
                    int baseheight = 0;
                    using (Bitmap first = new Bitmap(pictures[0]))
                    {
                        zoom = (double)first.Width / 800;
                        if (zoom <= 0.0) return;

                        basewidth = (int)(FleetTrimWidth * zoom);
                        baseheight = (int)(FleetTrimHeight * zoom);
                    }

                    //サイズの決定
                    int combinewidth = basewidth * FleetTrimColNum;
                    int combineheight = baseheight * (((pictures.Length - 1) / FleetTrimColNum) + 1);

                    using(Bitmap result = new Bitmap(combinewidth, combineheight))
                    {
                        using(Graphics g = Graphics.FromImage(result))
                        {
                            //全体を黒で塗りつぶし
                            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, result.Width, result.Height));

                            //個別にトリミングして貼り付け
                            foreach(int i in Enumerable.Range(0, pictures.Length))
                            {
                                using(Bitmap paste = new Bitmap(pictures[i]))
                                {
                                    //個別の倍率
                                    double srczoom = (double)paste.Width / 800;

                                    //座標変換
                                    Rectangle srcrect =
                                        new Rectangle(paste.Width - (int)(FleetTrimWidth * srczoom), (int)(FleetTrimY * srczoom),
                                            (int)(FleetTrimWidth * srczoom), (int)(FleetTrimHeight * srczoom));
                                    //座標のチェック
                                    if(srcrect.Left > paste.Width || srcrect.Top > paste.Height)
                                    {
                                        srcrect = Rectangle.Empty;
                                    }
                                    else
                                    {
                                        if (srcrect.Right > paste.Width) srcrect.Width = Math.Max(0, paste.Width - srcrect.X);
                                        if (srcrect.Bottom > paste.Height) srcrect.Height = Math.Max(0, paste.Height - srcrect.Y);
                                    }

                                    //コピー先の領域
                                    int x = i % FleetTrimColNum;
                                    int y = i / FleetTrimColNum;
                                    Rectangle destrect = new Rectangle(x * basewidth, y * baseheight, basewidth, baseheight);

                                    //コピー
                                    g.DrawImage(paste, destrect, srcrect, GraphicsUnit.Pixel);
                                }
                            }
                        }
                        //出力のファイル名
                        string extension = Path.GetExtension(pictures[0]);
                        string outfilename = string.Format("{0}_combined{1}", pictures[0].Replace(extension, ""), ".png");
                        //保存
                        result.Save(outfilename);
                    }
                });
        }

        //イベントハンドラー（共通）
        //DragEnter
        public static void ToolButton_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            }
            else if(e.Data.GetDataPresent(typeof(DragDropItem)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        //DragDrop
        public static void ToolButton_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] originalfiles = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
            DragDropItem listviewfiles = (DragDropItem)e.Data.GetData(typeof(DragDropItem));

            List<string> imagetemp = new List<string>();
            foreach(string x in originalfiles ?? new string[0])
            {
                //フォルダだったら
                if (!File.Exists(x)) continue;

                string[] safefiletype = new string[]
                {
                    "png", "bmp", "jpg", "jpeg", "gif",
                };

                //画像ファイル以外だったら（拡張子で判断）
                string extension = Path.GetExtension(x).Replace(".", "");
                if (Array.IndexOf(safefiletype, extension.ToLower()) == -1) continue;

                //拡張子偽装対策
                //　＞＞＞＞しない＜＜＜＜
                imagetemp.Add(x);
            }
            foreach(string x in (listviewfiles == null ? new List<string>(0) : listviewfiles.Texts))
            {
                if (!File.Exists(x)) continue;

                string[] safefiletype = new string[]
                {
                    "png", "bmp", "jpg", "jpeg", "gif",
                };

                //画像ファイル以外だったら（拡張子で判断）
                string extension = Path.GetExtension(x).Replace(".", "");
                if (Array.IndexOf(safefiletype, extension.ToLower()) == -1) continue;

                imagetemp.Add(x);
            }

            if (imagetemp.Count == 0) return;

            string[] imagefiles = imagetemp.OrderBy(x => x).ToArray();

            //本処理
            var button = sender as System.Windows.Forms.Button;

            CallBacks.SetControlEnabled(button, false);

            switch(button.Name)
            {
                case "button_tool_black":
                    FillBlackPortAsync(imagefiles).ContinueWith(_ =>
                        {
                            CallBacks.SetControlEnabled(button, true);
                            MessageBox.Show(string.Format("{0} 個のファイルの黒塗りが完了しました", imagefiles.Length));
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
                case "button_tool_black2":
                    FillBlackBattleAsync(imagefiles).ContinueWith(_ =>
                        {
                            CallBacks.SetControlEnabled(button, true);
                            MessageBox.Show(string.Format("{0} 個のファイルの黒塗りが完了しました", imagefiles.Length));
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
                case "button_tool_combine":
                    CombineAsync(imagefiles, Convert.ToInt32(button.Tag)).ContinueWith(_ =>
                        {
                            CallBacks.SetControlEnabled(button, true);
                            MessageBox.Show(string.Format("{0} 個のファイルを結合しました", imagefiles.Length));
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
                case "button_tool_trimcombine":
                    FleetTrimCombineAsync(imagefiles).ContinueWith(_ =>
                        {
                            CallBacks.SetControlEnabled(button, true);
                            MessageBox.Show(string.Format("{0} 個のファイルを結合しました", imagefiles.Length));
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
                case "button_tool_trimfleet":
                    TrimPortFleetAsync(imagefiles).ContinueWith(_ =>
                    {
                        CallBacks.SetControlEnabled(button, true);
                        MessageBox.Show(string.Format("{0} 個のファイルをトリミングしました", imagefiles.Length));
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
            }
        }
    }

    //D&D用のアイテム
    [Serializable]
    public class DragDropItem
    {
        public List<string> Texts { get; set; }

        public DragDropItem(ListView.SelectedListViewItemCollection lvic)
        {
            this.Texts = lvic.OfType<ListViewItem>().Select(x => x.SubItems[1].Text).ToList();
        }
    }
}
