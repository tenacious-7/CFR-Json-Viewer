<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>React JSON Viewer</title>

    <!-- React + Babel CDN -->
    <script crossorigin src="https://unpkg.com/react@17/umd/react.development.js"></script>
    <script crossorigin src="https://unpkg.com/react-dom@17/umd/react-dom.development.js"></script>
    <script src="https://unpkg.com/babel-standalone@6/babel.min.js"></script>

    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
        }
        pre {
            white-space: pre-wrap;
            word-wrap: break-word;
            max-height: 600px;
            overflow-y: auto;
            background-color: #f8f8f8;
            padding: 10px;
            border-radius: 6px;
            box-shadow: 0 0 5px rgba(0,0,0,0.1);
        }
    </style>
</head>
<body>
    <h2>📦 Decompressed JSON Viewer</h2>

    <div id="jsonViewer"></div>

    <script type="text/babel">
        function App() {
            const [jsonData, setJsonData] = React.useState("");

            React.useEffect(() => {
                fetch("http://localhost:5000/getdata")
                    .then((res) => res.json())
                    .then((data) => {
                        setJsonData(JSON.stringify(data, null, 2));
                    })
                    .catch((err) => {
                        setJsonData("❌ Failed to fetch data: " + err.message);
                    });
            }, []);

            return (
                <div>
                    <pre>{jsonData}</pre>
                </div>
            );
        }

        ReactDOM.render(<App />, document.getElementById("jsonViewer"));
    </script>
</body>
</html>
