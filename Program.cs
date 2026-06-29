using System;
using System.IO;
using System.Text;

namespace _2026_06_17_csharp_file_randomNumber_date;

class Program
{
    static void Main()
    {

        while (true)
        {
            Console.WriteLine("請選擇題目1-20:");
            String input = Console.ReadLine() ?? "";
            if (input == "q")
            {
                Console.WriteLine("==<已離開程序>==");
                break;
            }
            switch (input)
            {
                case "1": Q1(); break;
                case "2": Q2(); break;
                case "3": Q3(); break;
                case "4": Q4(); break;
                case "5": Q5(); break;
                case "6": Q6(); break;
                case "7": Q7(); break;
                case "8": Q8(); break;
                case "9": Q9(); break;
                case "10": Q10(); break;
                case "11": Q11(); break;
                case "12": Q12(); break;
                case "13": Q13(); break;
                case "14": Q14(); break;
                case "15": Q15(); break;
                case "16": Q16(); break;
                case "17": Q17(); break;
                case "18": Q18(); break;
                case "19": Q19(); break;
                case "20": Q20(); break;

                default: Console.WriteLine("沒有這一題!"); break;
            }
            Console.WriteLine();
            Console.WriteLine("==(輸入q離開)==");

        }
    }
    //檔案處理題目1
    //寫一篇中文歌的歌詞到到自己指定的文字檔(使用UTF-8編碼)。
    static void Q1()
    {
        Console.WriteLine("-----");
        using (var sr = new StreamReader("q1.txt", Encoding.UTF8))
        {
            using (var sw = new StreamWriter("copyq1.txt", false, Encoding.UTF8))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
            }
        }
        Console.WriteLine("已複製完成，請查看copyq1.txt");
    }
    //檔案處理題目2
    //讀取1.txt 顯示在畫面上。
    static void Q2()
    {
        Console.WriteLine("-----");
        using (var sr = new StreamReader("q1.txt", Encoding.UTF8))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
    //檔案處理補充1
    //寫入九九乘法表資料到一個文字檔到自己指定的文字檔。
    static void Q3()
    {
        Console.WriteLine("-----");
        using (var sw = new StreamWriter("q3.txt", false))
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    sw.Write($"{i}*{j}={i * j,-4}");
                }
                sw.WriteLine();
            }
        }
        Console.WriteLine($"轉換完成請查看q3.txt");
    }
    //檔案處理補充2
    //讀取1.txt 顯示在畫面上，並將1.txt 裡的阿拉伯數字，轉換成中文數字(壹、貳、叁、肆…..)，並儲存到指定的路徑。(UTF-8)
    static void Q4()
    {
        Console.WriteLine("-----");
        // 1. 讀檔 + 顯示Encoding.UTF8
        string content = File.ReadAllText("q3.txt", Encoding.UTF8);
        string table = "零壹貳參肆伍陸柒捌玖";
        var result = new StringBuilder();
        foreach (char c in content)
        {
            if (c >= '0' && c <= '9')
            {
                result.Append(table[c - '0']);
            }
            else
            {
                result.Append(c);
            }
        }
        string output = result.ToString();
        string path = "convert.txt";
        File.WriteAllText(path, output, new UTF8Encoding(false));
        Console.WriteLine($"轉換完成請查看{path}");
    }
    //檔案處理補充３
    //讀取fc4bb.csv，並將此資料轉成HTML TABLE 格式，並儲存到指定的HTML檔裡。
    static void Q5()
    {
        Console.WriteLine("-----");
        using (var sr = new StreamReader("fc4bb.csv", Encoding.UTF8))
        using (var sw = new StreamWriter("q5.html", false, Encoding.UTF8))
        {
            sw.WriteLine("<table border='1'>");
            string? line;
            bool isHeader = true;
            while ((line = sr.ReadLine()) != null)   // 一次拿一行
            {
                string[] cells = line.Split(',');     // 這一行用逗號切成欄
                sw.Write("<tr>");
                string tag = isHeader ? "th" : "td";
                foreach (string cell in cells)        // 逐欄印出來看
                {
                    sw.Write($"<{tag}>{cell}</{tag}>");
                }
                sw.WriteLine("</tr>");
                isHeader = false;
            }
            sw.WriteLine("</table>");
        }
    }
    //亂數題目1
    /*請隨機由0~99產生一個數字輸出。
Random rom= new Random();//亂數種子int I = rom.Next(0, 100);//回傳0-99的亂數*/
    static void Q6()
    {
        Console.WriteLine("-----");
        Random rom = new Random();
        int n = rom.Next(0, 100);
        Console.WriteLine(n);
    }
    //亂數題目2
    //請隨機由0~99產生10個數字輸出。
    static void Q7()
    {
        Console.WriteLine("-----");
        for (int i = 0; i < 10; i++)
        {
            Random rom = new Random();
            int n = rom.Next(0, 100);
            Console.WriteLine(n);
        }
    }
    //亂數題目３
    //隨機幫每位學員產生成績，並寫入文字檔(欄位之間用，分開，換行寫入下一筆)。
    static void Q8()
    {
        Console.WriteLine("-----");
        using (var sw = new StreamWriter("q8.txt", false, new UTF8Encoding(false)))
        {
            string[] arr = { "姓名", "國文", "數學", "英文", "歷史", "地理" };
            sw.WriteLine(string.Join(",", arr));
            string[] name = { "小明", "汪大東", "郭富城", "田勝傑", "羅傑" };
            Random rom = new Random();
            for (int i = 0; i < 5; i++)
            {
                sw.Write(name[i]);
                for (int j = 0, a = arr.Length; j < a - 1; j++)
                {
                    int n = rom.Next(0, 101);
                    sw.Write("," + n);
                }
                sw.WriteLine();
            }
        }
    }
    //亂數補充一
    //請設計樂透開獎程式。
    static void Q9()
    {
        Console.WriteLine("-----");
        // 1.先新增list 物件
        List<int> pool = new List<int>();
        // 2.for迴圈(樂透數字幾個）;
        for (int i = 1; i <= 50; i++) pool.Add(i);
        // 3.新增變數物件方法
        Random rom = new Random();
        //3.5特別數
        int sp = rom.Next(1, 11);
        Console.WriteLine("特別數是" + sp);
        // 4.for迴圈抽六個（抽過印出來，刪除抽過的）;
        for (int i = 1; i <= 6; i++)
        {
            int n = rom.Next(0, pool.Count);
            Console.WriteLine($"第{i}個數字是{pool[n]}");
            pool.RemoveAt(n);
        }
    }
    //亂數補充二
    //請在文字檔裡輸入所有午餐的店家，讀取文字檔，隨機抽出今天中午要吃哪一家。
    static void Q10()
    {
        Console.WriteLine("-----");
        string res = File.ReadAllText("q10.txt", new UTF8Encoding(false));
        string[] py = res.Split(",");
        Random rom = new Random();
        int n = rom.Next(0, py.Length);
        Console.WriteLine(py[n]);
    }
    //亂數補充三
    //請在文字檔裡輸入所有教室裡的學員名字，讀取文字檔，隨機抽出今天的值日生，抽過不能再被抽中，直到全部學員都被抽過，才可以再被抽。
    static void Q11()
    {
        Console.WriteLine("-----");
        string rosterFile = "students.txt";   // new game（初始狀態）
        string poolFile = "pool.txt";       // pool存檔點

        // 1. list當前遊戲，會先去檢查有沒有上一局的存檔點（且整理存檔點資料）
        List<string> pool = File.Exists(poolFile)
        ? new List<string>(File.ReadAllLines(poolFile, new UTF8Encoding(false)))
        : new List<string>();
        pool.RemoveAll(s => s.Trim() == "");
        // 2. 如果沒有存檔點表示，當前遊戲pool要從初始裝態new game抓初始狀態的遊戲下來（且整理new game資料）
        if (pool.Count == 0)
        {
            pool = new List<string>(File.ReadAllLines(rosterFile, new UTF8Encoding(false)));
            pool.RemoveAll(s => s.Trim() == "");
            Console.WriteLine("(上一輪抽完，已重置名單)");
        }

        // 3. 隨機抽一個(pool lsit範圍內)
        Random rnd = new();
        int idx = rnd.Next(0, pool.Count);
        Console.WriteLine("今日值日生:" + pool[idx]);
        // 4. 移除抽過的，把剩下的寫回檔案poolFile
        pool.RemoveAt(idx);
        File.WriteAllLines(poolFile, pool, new UTF8Encoding(false));
    }
    //日期題目1
    //顯示現在日期與時間。
    static void Q12()
    {
        Console.WriteLine("-----");
        DateTime currentTime = DateTime.Now;
        Console.WriteLine("現在的系統時間是：" + currentTime);
    }
    //日期題目2
    //顯示再過30天為哪一天。
    static void Q13()
    {
        Console.WriteLine("-----");
        DateTime today = DateTime.Today;
        DateTime future = today.AddDays(30);
        Console.WriteLine("一個月後時間是:" + future);
    }
     //日期題目3
    //顯示24小時前的年月日時分秒。
    static void Q14()
    {
        Console.WriteLine("-----");
        DateTime today = DateTime.Now;
        DateTime future = today.AddDays(-1);
        Console.WriteLine("24小時前時間是:" + future);
    }
    //日期題目4
    //取得目前是幾月。
    static void Q15()
    {
        Console.WriteLine("-----");
        DateTime today = DateTime.Now;
        int future = today.Month;
        Console.WriteLine("取得目前是幾月:" + future + "月");
    }
    //日期題目5
    //取得明年是否為閏年。(可以試試民國)
    static void Q16()
    {
        Console.WriteLine("-----");
        DateTime today = DateTime.Now;
        int future = today.Year + 1;
        Console.Write($"明年，民國{future - 1911}年是：");
        if (future % 400 == 0)
        {
            Console.WriteLine("閏年");
        }
        else if (future % 100 == 0)
        {
            Console.WriteLine("平年");
        }
        else if (future % 4 == 0)
        {
            Console.WriteLine("閏年");
        }
        else
        {
            Console.WriteLine("平年");
        }
    }
    //日期題目6
    //取得離2025年1月1日還有幾天。
    static void Q17()
    {
        Console.WriteLine("-----");
        DateTime target = new DateTime(2025, 1, 1);
        DateTime today = DateTime.Today;
        TimeSpan diff = target - today;     // 日期 - 日期 = TimeSpan
        int days = Math.Abs(diff.Days);                  // 從 TimeSpan 挖出「天數」
        Console.WriteLine("相差 " + days + " 天");
    }
    //日期補充一
    /*星期一，猴子穿新衣，
星期二，猴子肚子餓，
星期三，猴子去爬山，
星期四，猴子看電視，
呈期五，猴子去跳舞，
星期六，猴子去斗六，
星期日，猴子過生日。
請顯示今天猴子做甚麼事。
*/
    static void Q18()
    {
        string[] things = { "過生日", "穿新衣", "肚子餓", "去爬山", "看電視", "去跳舞", "去斗六" };
        //   對應星期：      日(0)    一(1)   二(2)   三(3)   四(4)   五(5)   六(6)
        string thing = things[(int)DateTime.Today.DayOfWeek];
        Console.WriteLine("今天猴子" + thing);
    }

    //日期補充二
    //輸入‘兩個日期，輸出兩個日期相差幾天。
    static void Q19()
    {
        Console.WriteLine("-----");
        Console.Write("請輸入第一個日期 (例 2025-01-01)：");
        string s1 = Console.ReadLine() ?? "";
        Console.Write("請輸入第二個日期 (例 2025-12-31)：");
        string s2 = Console.ReadLine() ?? "";

        // TryParse：轉成功才繼續，避免亂打字當掉
        if (DateTime.TryParse(s1, out DateTime d1) &&
            DateTime.TryParse(s2, out DateTime d2))
        {
            TimeSpan diff = d2 - d1;            // 日期 - 日期 = TimeSpan
            int days = Math.Abs(diff.Days);    // 取絕對值，不分先後
            Console.WriteLine($"兩個日期相差 {days} 天");
        }
        else
        {
            Console.WriteLine("日期格式錯誤！");
        }
    }
    //日期補充三
    /*兩光法師時常替人占卜，由於他算得又快有便宜，因此生意源源不絕，時常大排長龍，他想算 得更快一點，因此找了你這位電腦高手幫他用電腦來加快算命的速度。
　　他的占卜規則很簡單，規則是這樣的，隨機產生一個今年日期，然後依照下面的公式：
M=月D=日S=(M*2+D)%3
得到 S 的值，再依照 S 的值從 0 到 2 分別給與 普通、吉、大吉 等三種不同的運勢，輸出運勢。
*/
    static void Q20()
    {
        Console.WriteLine("-----");
        Random rom = new Random();

        // 1. 隨機產生今年的一個日期
        int year = DateTime.Today.Year;
        int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
        DateTime date = new DateTime(year, 1, 1).AddDays(rom.Next(0, daysInYear));

        // 2. 拿出月、日
        int M = date.Month;
        int D = date.Day;

        // 3. 套公式
        int S = (M * 2 + D) % 3;

        // 4. 依 S 輸出運勢
        string[] fortune = { "普通", "吉", "大吉" };
        Console.WriteLine($"占卜日期:{date:yyyy/MM/dd}");
        Console.WriteLine($"M={M} D={D} S={S}");
        Console.WriteLine($"今日運勢:{fortune[S]}");
    }

}