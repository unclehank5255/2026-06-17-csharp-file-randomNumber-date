# C# 檔案讀寫 + 常用觀念 筆記

> 這份筆記涵蓋讀檔、寫檔、編碼、`new`、`string` 家族、`Split/Join`、`Random`、迴圈，
> 並在最後整理「我實際犯過的錯」。寫程式前/卡住時翻這份。

---

## 目錄
1. [`new` 到底何時要寫](#1-new-到底何時要寫最常搞混)
2. [`var` vs 明確型別](#2-var-vs-明確型別)
3. [讀檔](#3-讀檔)
4. [寫檔](#4-寫檔)
5. [編碼與 BOM](#5-編碼與-bom)
6. [`string` vs `string[]` vs `StringBuilder`](#6-string-vs-string-vs-stringbuilder)
7. [`Split` 與 `string.Join`](#7-split-與-stringjoin)
8. [`Random` 亂數](#8-random-亂數)
9. [迴圈：foreach vs for](#9-迴圈foreach-vs-for)
10. [我實際犯過的錯（踩雷紀錄）](#10-我實際犯過的錯踩雷紀錄)
11. [常用範本（背這些就夠）](#11-常用範本背這些就夠)

---

## 1. `new` 到底何時要寫（最常搞混）

**核心判斷：有沒有一個函式會「回傳」這個東西給我？**

- **有** → 直接接，不用 `new`（東西是函式做好的）
- **沒有** → 自己蓋，要 `new`

```csharp
string content = File.ReadAllText("a.txt", Encoding.UTF8); // 函式回傳 → 不用 new
string[] cells = line.Split(',');                          // 函式回傳 → 不用 new

var sb = new StringBuilder();          // 沒人回傳 → 自己蓋 → 要 new
var sw = new StreamWriter("a.txt");    // 要 new
var enc = new UTF8Encoding(false);     // 要 new
string[] arr = new string[3];          // 開空陣列 → 要 new
```

**重要觀念**：「沒寫 `new` ≠ 沒佔記憶體」。
`line.Split(',')` 一樣會配記憶體，只是那個 `new` 藏在 `Split` 內部、它幫你做了。
（比喻：自己烤麵包 = 你 `new`；買現成麵包 = 店家內部 `new` 過，你只是接過來。）

---

## 2. `var` vs 明確型別

`var` **不是型別**，是「編譯器你自己推斷型別」。下面兩行完全一樣：

```csharp
var          sw = new StreamWriter("a.txt");
StreamWriter sw = new StreamWriter("a.txt");
```

**習慣**：右邊一看就知道型別（如 `new StreamWriter()`）→ 用 `var`；
右邊看不出型別（如 `File.ReadAllText()` 回傳什麼？）→ 寫出 `string` 讓自己看得懂。

---

## 3. 讀檔

### 讀取方法家族

| 方法 | 一次給你 | 回傳型別 | 何時用 |
|------|---------|----------|--------|
| `File.ReadAllText(路徑, 編碼)` | 整份黏一坨 | `string` | 整份內容一起處理 |
| `File.ReadAllLines(路徑, 編碼)` | 整份切成一行一格 | `string[]` | 一行一行處理（CSV 適合） |
| `StreamReader` + `ReadLine()` | 一行 | `string?` | 邊讀邊處理 / 大檔 |
| `StreamReader` + `ReadToEnd()` | 整份 | `string` | 已開 sr 想一次拿全部 |
| `StreamReader` + `Read()` | 一個字元 | `int` | 幾乎用不到 |

### StreamReader 標準寫法

```csharp
using (var sr = new StreamReader("a.txt", Encoding.UTF8))
{
    string? line;
    while ((line = sr.ReadLine()) != null)   // 讀完回傳 null → 結束
    {
        // 處理 line（line 已自動去掉換行符號）
    }
}
```

- **一定要包 `using`**：用完自動關檔。
- `while` 條件做兩件事：①讀一行存進 `line` ②檢查是不是 `null`（沒了）。
- 換行交給 `ReadLine`/`ReadAllLines` 處理，**別自己 `Split('\n')`**（會殘留 Windows 的 `\r`）。

---

## 4. 寫檔

### 寫入方法家族

| 寫法 | 說明 |
|------|------|
| `File.WriteAllText(路徑, 字串, 編碼)` | 一整串一次寫完（懶人版） |
| `File.WriteAllLines(路徑, 字串陣列, 編碼)` | 一個元素一行 |
| `StreamWriter` + `Write` / `WriteLine` | 邊算邊一行一行寫 |

### StreamWriter 標準寫法

```csharp
using (var sw = new StreamWriter("a.txt", false, new UTF8Encoding(false)))
{
    sw.WriteLine("第一行");
    sw.Write("不換行");
}   // 離開 using → 自動關檔存檔
```

三個參數：`new StreamWriter(檔名, 覆蓋?, 編碼)`
- 第 2 個 `false` = **覆蓋**（清空重寫）；`true` = **附加**（接在後面）。

### `Write` vs `WriteLine`

```csharp
sw.Write("abc");      // 寫完不換行，下一個接著貼
sw.WriteLine("abc");  // 寫完換行
sw.WriteLine();       // 只換行，不寫東西
```

範例：
```csharp
sw.Write("a"); sw.Write("b"); sw.WriteLine(); sw.Write("c");
// 檔案內容：
// ab
// c
```

### File 懶人版 vs StreamWriter（等價）

```csharp
// 這兩段做一樣的事
File.WriteAllText("a.txt", output, new UTF8Encoding(false));

using (var sw = new StreamWriter("a.txt", false, new UTF8Encoding(false)))
    sw.Write(output);
```
- 有一整串現成的 → `File.WriteAllText`
- 要邊算邊一行行寫 → `StreamWriter`

---

## 5. 編碼與 BOM

| | 讀檔 | 寫檔 |
|--|------|------|
| 建議編碼 | `Encoding.UTF8` | `new UTF8Encoding(false)` |
| 原因 | BOM 會自動吃掉，免煩惱 | 主動關 BOM，避免害下游 |

### BOM 是什麼
- BOM = 檔案開頭一個**看不見的記號**，UTF-8 的 BOM 是 3 個 byte：`EF BB BF`（字元 `U+FEFF`）。
- `Encoding.UTF8` 寫檔會**偷偷加** BOM；`new UTF8Encoding(false)` 的 `false` = **不加**。

### BOM 會害你的真實例子（實測過）
```
with_bom.txt    : EF-BB-BF-31-32-33   ← 內容 "123"，但前面多 3 byte
without_bom.txt :          31-32-33   ← 乾淨的 "123"
```
若程式沒自動去 BOM：
- `text == "123"` → **false**（多一個隱形字元）
- `int.Parse(text)` → **報錯**：`input string '﻿123' was not in a correct format`
- 第一個字元變成 `U+FEFF` 而不是 `1`

**結論**：中文一定指定 UTF-8；寫檔用 `new UTF8Encoding(false)` 最安全。

---

## 6. `string` vs `string[]` vs `StringBuilder`

| | `string` | `string[]` | `StringBuilder` |
|--|----------|-----------|-----------------|
| 是什麼 | 一條字串 | 一排盒子，每格一條字串 | 一條會長大的字串 |
| 索引拿到 | `s[0]` → `char`（單字元） | `arr[0]` → `string` | 不用索引 |
| 放東西 | 宣告時給，**不能改某格** | `arr[0]="x"`，可改 | `sb.Append("x")` 往後貼 |
| 取出 | 直接用 | `arr[i]` / `foreach` | `sb.ToString()` |
| 大小 | 固定 | 建立時固定 | 會長大 |
| 適合 | 單一字串 | 已知的多個分開字串（如 CSV 一行的各欄） | 把很多片段拼成一大坨（如組 HTML） |

關鍵差別範例：
```csharp
string   table = "零壹貳";   table[1];  // → '壹'  (char)
string[] names = {"小明","小華"}; names[1]; // → "小華" (string)
```

**陷阱**：`string s = "小明,小華";` 時 `s[0]` 是 `'小'`（一個字），**不是** "小明"！
要拿「一個一個名字」必須用 `string[]`。

---

## 7. `Split` 與 `string.Join`（互為相反）

```csharp
// Split：把一條字串「切開」成 string[]
string[] cells = "小明,90,85".Split(',');   // → ["小明","90","85"]

// Join：把 string[] 用符號「黏成」一條字串
string line = string.Join(",", cells);       // → "小明,90,85"
```

- `Split` 回傳 `string[]` → **一定要用 `string[]` 接**（不能用單個 `string` 接）。
- `string.Join` 的好處：逗號**只放在元素之間**，頭尾不會多 → 不會有「結尾多逗號」的 bug。
- 前提：`Join` 需要資料**已經在陣列裡**。

CSV 兩層切法（每層都是 string[]）：
```csharp
string[] lines = File.ReadAllLines("a.csv", Encoding.UTF8); // 第1層：切成每一列
string[] cells = lines[1].Split(',');                       // 第2層：一列切成各欄
```

> 進階：若欄位內容自己含逗號（被引號包住，如 `"Anytown, USA"`），
> `Split(',')` 會切錯，要用正則或 CSV 函式庫。乾淨資料用 `Split(',')` 就好。

---

## 8. `Random` 亂數

```csharp
Random rom = new Random();   // 做一台亂數機器（要 new）
int n = rom.Next(0, 100);    // 吐一個 0~99 的數
```

### ⚠️ 範圍陷阱：頭包含、尾不包含
```csharp
rom.Next(0, 100)   // → 0 ~ 99   （抽不到 100！）
rom.Next(0, 101)   // → 0 ~ 100  （要含 100 寫這個）
```

### ⚠️ `Random` 要放迴圈「外面」
```csharp
Random rom = new Random();        // ✅ 只一台，放所有迴圈外
for (int i = 0; i < 10; i++)
    Console.WriteLine(rom.Next(0, 100));
```
- 原理：`Random` 是「種子決定的固定序列」，`.Next()` 每次往後抽一個。
- 一台機器一直抽 → 數字才會往下走、不重複。
- 舊版 .NET 在迴圈裡 `new` 會因「同時間=同種子」吐出**全一樣**的數字（經典坑）。
- 新版 .NET 不會重複，但**仍該放外面**：省資源、合習慣、換語言/舊版不踩雷。

---

## 9. 迴圈：foreach vs for

```csharp
// foreach：一個一個拿出來用（不在乎第幾個）
foreach (char c in content) { ... }   // c 每圈自動變成下一個字元
foreach (string name in names) { ... }

// for：需要知道「第幾個」時用
for (int i = 0; i < names.Length; i++) { ... names[i] ... }
```
- `foreach (char c in content)` 的 `c` 是「目前輪到的那個」，**不是要找的目標**；判斷靠裡面的 `if`。
- 要「跨圈記住的東西」（如旗標 `isHeader`、`StringBuilder`、`Random`）→ 放迴圈**外面**。
- 別寫死數字，用 `.Length`（`names.Length`、`arr.Length`）。

---

## 10. 我實際犯過的錯（踩雷紀錄）

| # | 我寫錯的 | 為什麼錯 | 正確 |
|---|---------|----------|------|
| 1 | `string table = "壹貳參..."`（少了「零」） | 對照表只有 9 格，數字 9 去拿 `table[9]` → `IndexOutOfRangeException` | 從「零」開始放滿 10 格：`"零壹貳參肆伍陸柒捌玖"` |
| 2 | `string name = "小明,小華";` 然後 `name[i]` | `name[i]` 是第 i 個**字元**不是名字 | 用 `string[] names = {"小明","小華"};` |
| 3 | `text[count] = line.Split(',');` | 左邊一格只能裝一個 `string`，右邊是整個 `string[]` | 用 `string[] cells = line.Split(',');` |
| 4 | `string[] text;` 沒 `new` 就 `text[count]=...` | 陣列要先開空間 | `new string[n]`，或乾脆一行一行處理不要存 |
| 5 | `Random rom = new Random();` 放在迴圈裡 | 浪費資源、舊版會吐重複數字 | 放所有迴圈外，只 `new` 一次 |
| 6 | `using (var sr = new StreamReader("a", UTF8Encoding(false)))` | `UTF8Encoding(false)` 漏了 `new` | 補 `new`，或讀檔直接用 `Encoding.UTF8` |
| 7 | 標題列用 `,4` 空格對齊、資料列用逗號 | 兩種格式混用 → 不是乾淨 CSV | 統一用逗號（`string.Join(",", arr)`） |
| 8 | 標題 11 欄、資料 6 欄 | 欄數不一致 | 讓「標題科目數 = 成績個數」對齊 |

> 另外記住：**CSV 視覺上欄位沒對齊是正常的**（給程式讀的，不是給人眼看整齊）。
> 把數字轉中文後排版跑掉也是正常——那是全形/半形寬度不同造成的，不是 bug。

---

## 11. 常用範本（背這些就夠）

```csharp
// === 讀：整份成字串 ===
string content = File.ReadAllText("a.txt", Encoding.UTF8);

// === 讀：一行一格陣列 ===
string[] lines = File.ReadAllLines("a.txt", Encoding.UTF8);

// === 讀：邊讀邊處理 ===
using (var sr = new StreamReader("a.txt", Encoding.UTF8))
{
    string? line;
    while ((line = sr.ReadLine()) != null)
    {
        string[] cells = line.Split(',');   // 需要切欄時
        // 處理...
    }
}

// === 寫：一整串一次寫 ===
File.WriteAllText("out.txt", output, new UTF8Encoding(false));

// === 寫：邊算邊寫 ===
using (var sw = new StreamWriter("out.txt", false, new UTF8Encoding(false)))
{
    sw.WriteLine(string.Join(",", arr));    // 一列用逗號黏起來
}

// === 亂數 ===
Random rom = new Random();        // 放迴圈外
int n = rom.Next(0, 101);         // 0~100（尾巴不含，要含就 +1）
```
