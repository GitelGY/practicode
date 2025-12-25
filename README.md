# 🚀 Software Development Portfolio
### תשתיות מערכת, אלגוריתמיקה ואפליקציות Fullstack

ריכוז פרויקטים נבחרים המציגים יכולות פיתוח תוכנה במגוון שכבות הטכנולוגיה – החל מכלים ברמת מערכת ההפעלה ועד למערכות Web מורכבות ומאובטחות.

---

## 1. 📦 BundleCli | .NET Automation Tool
**כלי CLI לאוטומציה, ניהול ואריזת קבצי קוד.**

הכלי פותח כפתרון לצורך בריכוז פרויקטים מרובי קבצים לתוך קובץ מארז (Bundle) יחיד ומאורגן. הפרויקט מדגים יכולת עבודה מול File System ואוטומציה של תהליכים באמצעות שורת הפקודה.

**פיצ'רים מרכזיים:**
* אריזה מבוססת שפות תכנות והחרגה אוטומטית של תיקיות מערכת (bin, debug).
* מחולל קבצי תגובה (.rsp) אינטראקטיבי המפשט הרצה של פקודות מורכבות.
* עיבוד תוכן: הסרת שורות ריקות, מיון לפי סוג או שם, והוספת Metadata של מקור הקוד.

**טכנולוגיות:** .NET Core, System.CommandLine.

---

## 2. 🌐 Html Serializer & Query Engine
**מנוע לניתוח (Parsing) ותשאול מבני נתונים היררכיים.**

פרויקט תשתיתי המממש Parser עצמאי ההופך טקסט HTML גולמי לעץ אובייקטים (DOM Tree). המערכת מאפשרת לבצע שאילתות על המבנה שנוצר בצורה דומה ל-CSS Selectors.

**אתגרים אלגוריתמיים:**
* **Building the Tree:** המרת טקסט לעץ אובייקטים תוך זיהוי היררכיה ותגיות Self-closing.
* **Query Engine:** מימוש מנוע חיפוש התומך ב-Selectors מורכבים (תגיות, ID ומחלקות).
* **Performance:** שימוש ב-Queue לסריקה בטוחה (למניעת Stack Overflow) ו-HashSet למניעת כפילויות.

**טכנולוגיות:** #C, Regex, Singleton Pattern, LINQ.

---

## 3. ✅ פרויקט 3 | ToDo List
**אפליקצית Fullstack לניהול משימות הכוללת Server, Client ו-DB.**

* **Minimal API:** פיתוח API תכליתי ורזה בשימוש ב-`Program.cs` בלבד, המותאם לארכיטקטורת MicroServices.
* **Dotnet CLI:** שימוש בשורת הפקודה לפיתוח חוצה פלטפורמות (Cross-platform) ללא תלות ב-Visual Studio.
* **DB First:** התחברות למסד נתונים MySQL באמצעות Entity Framework Core ושימוש בפקודת `scaffold` ליצירת מחלקות תואמות ל-DB.
* **Security & Auth:** מימוש מנגנון הזדהות עם JWT ב-API ובקליינט, כולל שימוש ב-Axios Interceptors לניהול שגיאות (401) והרשאות.

---

## 4. ☁️ פרויקט 4 | Deploy Apps
**העלאה לענן ופרסום המערכת בסביבת Production.**

שלב זה התמקד בהפיכת האפליקציה לזמינה ברשת תוך ניהול תשתיות ענן מורכבות:
* **Render:** פרסום אפליקציית הקליינט כ-Static Site ואפליקציית הסרבר כ-Web Service באמצעות Docker.
* **CleverCloud:** הקמת מסד נתונים MySQL בענן וחיבורו לסרבר.
* **Environment Variables:** ניהול משתני סביבה (Connection Strings וכתובות API) להפרדה בין סביבת הפיתוח לענן.

🔗 **לצפייה באפליקציה:** [https://client-7i8u.onrender.com/](https://client-7i8u.onrender.com/)

---

## 5. 🚀 GitHub Portfolio Explorer & Caching Engine
**מערכת Fullstack להצגה וחיפוש פרויקטים בזמן אמת מול GitHub API.**

הפרויקט מדגים פיתוח שרת API המתקשר עם מערכות חיצוניות תוך שימוש במנגנוני אופטימיזציה מתקדמים:
* **Smart Event-Based Caching:** מימוש מנגנון מטמון (Memory Cache) חכם המבצע בדיקת פעילות (Activity Events) מול שרת ה-API כדי להחליט אם לרענן את הנתונים.
* **Decorator Pattern:** שימוש בתבנית העיצוב Decorator (באמצעות **Scrutor**) המאפשרת הלבשת שכבת ה-Caching על שירות ה-API הקיים באופן מודולרי.
* **Data Transformation:** שימוש ב-DTOs ומיפוי נתונים ב-Controller לצורך החזרת אובייקטים ממוקדים וקלילים ל-Frontend.

**טכנולוגיות:** .NET 8, Octokit, MemoryCache, Scrutor, Vanilla JS.

---

תיק עבודות זה מהווה סיכום של יכולות פיתוח מודרניות ועדכניות שיושמו לאורך הפרויקטים, תוך דגש על מודולריות, אבטחת מידע ויציבות המערכת.

**כל הזכויות שמורות © 2025**