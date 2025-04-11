# 📦  Solving Real-World Performance Issues with Lazy Loading (React + .NET Core)

This project is designed to **store large JSON data in compressed form** inside SQL Server and **decompress it on-demand via an API**. The decompressed JSON is then displayed in a scrollable table using **React (via CDN)** on a static HTML page.

This is useful in scenarios where large datasets need to be stored efficiently and lazily rendered on the frontend without loading everything at once.

---

## 🎯 Project Goals

- ✅ Efficiently store large JSON data using GZip compression
- ✅ Build a .NET 6 Minimal API to decompress and serve the data
- ✅ Use plain React in an `index.html` file (no CRA or Vite required)
- ✅ Support lazy loading (infinite scroll) on the frontend
- ✅ Easily extendable for future pagination/compression endpoints

---

## 🧠 Technologies Used

| Tech             | Role                         |
|------------------|------------------------------|
| .NET 6           | Backend Minimal API           |
| SQL Server       | Data Storage (Compressed JSON)|
| GZip Compression | Compress/Decompress large JSON|
| React (CDN)      | Frontend UI                  |
| HTML/CSS         | Static layout + styling      |

---

## 🏗️ Project Structure

CFR-Json-Viewer/ │ ├── Program.cs # Main backend logic (API + decompression) ├── wwwroot/ │ └── index.html # React frontend with infinite scroll ├── json/ │ └── sample.json # JSON used for compression (optional) ├── README.md # You're reading it now 😊

sql
Copy
Edit

---

## 🧾 Database Setup

Use the following schema to create your table in SQL Server:

```sql
CREATE TABLE tblCFRjsondata (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CompressedCaseDetails VARBINARY(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);
🔐 Backend Logic - Program.cs
🔹 Compression Mode
When the app runs without any --api argument, it:

Reads a local JSON file (or hardcoded content)

Compresses it using GZip

Inserts it into the database (tblCFRjsondata)

🔹 API Mode
When the app runs with --api argument, it:

Starts a web server on http://localhost:5000

Exposes a GET /getdata endpoint

Decompresses the latest row from the database

Sends the JSON data back as a proper response

Example usage:

bash
Copy
Edit
dotnet run -- --api
🌐 API Endpoint
📍 GET /getdata
Returns: Decompressed JSON from the latest row in tblCFRjsondata

Content-Type: application/json

🖥️ Frontend - wwwroot/index.html
This is a static page using:

React (via CDN)

Fetches data from /getdata

Renders it inside a scrollable HTML table

Implements infinite scroll (client-side lazy loading)

No bundlers, no node/npm required!

🔁 How Lazy Loading Works
When the page loads, it fetches JSON from /getdata.

It displays the first 20 items.

When the user scrolls to the bottom, it loads 20 more.

Continues until all items are rendered.

Pagination is handled on the client side using slicing:

js
Copy
Edit
const nextItems = allData.slice(0, nextPage * itemsPerPage);
⚙️ How to Run the Project
Step 1: Clone the Repository
bash
Copy
Edit
git clone https://github.com/tenacious-7/CFR-Json-Viewer.git
cd CFR-Json-Viewer
Step 2: Run the Backend
✅ Option A: Compress JSON and Insert into DB
bash
Copy
Edit
dotnet run
✅ Option B: Start the API to serve data
bash
Copy
Edit
dotnet run -- --api
Step 3: View in Browser
Visit:

arduino
Copy
Edit
http://localhost:5000
📦 Example JSON Preview
Once fetched, the JSON will be decompressed and rendered in tabular format with headers.

🧪 Future Enhancements
 Add support for query parameters: ?page=1&limit=20

 Add endpoint to fetch raw compressed data

 Create an Admin Panel to upload/replace JSON

 Add server-side pagination and filters

 Add a download as .json feature

 Add unit testing and CI/CD workflow

🙋‍♂️ Author
Durgesh Kumar Pandey

🔗 LinkedIn

💻 GitHub

📧 pandeydurgesh21jan2000@gmail.com
