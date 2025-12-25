# 🚀 GitHub Portfolio Explorer

מערכת Full-stack המאפשרת למשתמשים להציג את פורטפוליו הפרויקטים שלהם מ-GitHub בזמן אמת, וכן לחפש פרויקטים ומשתמשים אחרים. המערכת כוללת מנגנון **Caching** חכם מבוסס אירועים לשיפור ביצועים.

## ✨ תכונות עיקריות
* **הפורטפוליו שלי:** הצגת כל הריפוזיטורים האישיים כולל שפות תכנות, כוכבים (Stars) וכמות Pull Requests.
* **חיפוש משתמשים:** חיפוש דינמי של משתמשים אחרים ב-GitHub והצגת הפרויקטים שלהם.
* **Caching חכם:** שימוש ב-`MemoryCache` עם בדיקת פעילות (Activity Events) כדי למנוע קריאות מיותרות ל-API של GitHub.
* **ממשק משתמש (UI):** דף HTML נקי, מגיב (Responsive) ומעוצב המציג כרטיסיות פרויקטים.

## 🛠 טכנולוגיות
### Backend:
* **ASP.NET Core 8.0 Web API**
* **Octokit.net:** ספרייה רשמית לעבודה מול ה-API של GitHub.
* **Scrutor:** למימוש תבנית ה-Decorator בצורה אלגנטית.
* **MemoryCache:** לניהול זיכרון מטמון בשרת.

### Frontend:
* **HTML5, CSS3, JavaScript (Vanilla)**
* **Fetch API:** לתקשורת אסינכרונית מול השרת.

## 🏗 ארכיטקטורה
המערכת בנויה לפי עקרונות הנדסת תוכנה מתקדמים:
1.  **Repository Pattern / Service Layer:** הפרדה בין ה-Controller ללוגיקת התקשורת מול GitHub.
2.  **Decorator Pattern:** הוספת יכולות ה-Caching לשירות הקיים ללא שינוי הקוד המקורי שלו.
3.  **DTOs (Data Transfer Objects):** החזרת אובייקטים ממוקדים וקלילים לדפדפן למניעת שגיאות המרה וביצועים טובים יותר.



## 🚀 הוראות הרצה

### 1. דרישות מוקדמות
* Visual Studio 2022.
* GitHub Personal Access Token (PAT).

### 2. הגדרות (Secrets)
יש להוסיף את פרטי ה-GitHub שלך לקובץ `secrets.json` או `appsettings.json`:
```json
{
  "GitHubOptions": {
    "Username": "YOUR_GITHUB_USERNAME",
    "Token": "YOUR_PERSONAL_ACCESS_TOKEN"
  }
}