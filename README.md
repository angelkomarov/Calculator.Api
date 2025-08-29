# 📐 Calculator.Api

## 📌 Description
**Calculator.Api** is an example **ASP.NET Core C# calculator service** that demonstrates common concepts of **serialization, recursion, encapsulation, inheritance, and interfaces**.  

It provides a flexible API that:  
- Accepts **HTTP POST** requests with either **XML** or **JSON** input.  
- Parses input using **serialization** (`System.Text.Json` & `System.Xml.Serialization`).  
- Uses **interfaces, inheritance, and encapsulation** for extensibility.  
- Returns results as **XML** or **JSON**, depending on the request’s `Accept` header.  

---

## 🚀 Features
- 🔄 **Serialization**  
  - JSON via `System.Text.Json`  
  - XML via `System.Xml.Serialization`  

- 🔁 **Recursion**  
  - Operations can be **nested inside each other**.  
  - Evaluation is handled **recursively**, so child operations are evaluated first, then combined into the parent operation.  

- 🧩 **Encapsulation**  
  - `BaseOperation` hides evaluation details.  

- 🏗 **Inheritance**  
  - Specific operations (e.g., `PlusOperation`, `MultiplicationOperation`) inherit from a common base.  

- 🔌 **Interface Design**  
  - `IOperation` provides a **plug-and-play strategy** for new operations.  
  - `ICalculatorService` supports **dependency injection (DI)**.  

- ➕ **Extensibility**  
  - Add new operations (e.g., **Subtract**) by simply creating a new C# class without modifying the input structure.  

---


## ⚙️ Why Custom Deserialization?
The **input request structure is fixed and cannot be changed**.  
- JSON requests use **`Rootobject`** as the root element.  
- XML requests use **`Maths`** as the root element.  

Because the structures differ, **default model binding fails**.  
To handle this while keeping the **request format unchanged**, the project:  
- **Bypasses** `[FromBody]` binding.  
- **Manually deserializes** the request body depending on the `Content-Type` header.  

✅ This ensures that clients can continue sending **both XML and JSON requests** in their existing format.  

---

## 📄 XML Formatter Support
To enable both XML and JSON request/response:  
- XML formatters are **registered in `Program.cs`**.  
- Clients must set proper **Content-Type** and **Accept** headers.  

---

## 📥 Sample Requests

### JSON Request
```json
{
  "Maths": {
    "Operation": {
      "@ID": "Plus",
      "Value": [
        "2",
        "3"
      ],
      "Operation": {
        "@ID": "Multiplication",
        "Value": [
          "4",
          "5"
        ]
      }
    }
  }
}
```
### XML Request
```xml
<?xml version="1.0" encoding="UTF-8"?>
<Maths>
  <Operation ID="Plus">
    <Value>2</Value>
    <Value>3</Value>
    <Operation ID="Multiplication">
      <Value>4</Value>
      <Value>5</Value>
    </Operation>
  </Operation>
</Maths>
```
---

## 🧪 Unit Tests (MSTest)

The project uses **MSTest** along with **Moq** and `WebApplicationFactory` to validate functionality at multiple levels.  

### 1️⃣ Service-Level Tests (`CalculatorServiceTests.cs`)  
Validates the **core calculation engine**:  

| Test Method | Purpose |
|-------------|---------|
| `Evaluate_PlusOperation_ReturnsCorrectSum` | Ensures addition returns the correct result (e.g., `2 + 3 = 5`). |
| `Evaluate_MultiplicationOperation_ReturnsCorrectProduct` | Ensures multiplication returns the correct result (e.g., `4 × 5 = 20`). |
| `Evaluate_NestedOperation_EvaluatesRecursively` | Confirms **recursive evaluation** of nested operations (e.g., `2 + 3 + (4 × 5) = 25`). |
| `Evaluate_UnknownOperation_ThrowsException` | Verifies robust error handling when an unsupported operation (e.g., `Divide`) is used. |
| `Evaluate_HandlesEmptyValuesAndNestedOnly` | Ensures correct fallback when values are missing but a nested operation exists (e.g., `Plus` with no values but a nested `Multiplication(3,3,3)` yields `27`). |

---

### 2️⃣ API Unit Test with Mocking (`CalculateApiTests.cs`)  
Validates the API controller **integration with the calculator service**, using **Moq**:  

| Test Method | Purpose |
|-------------|---------|
| `PostJsonRequest_UsesMockedCalculator` | Ensures the controller correctly calls the calculator service. The mock forces a return value (`1234`) and verifies `Evaluate()` is invoked exactly once. |

---

### 3️⃣ End-to-End Integration Tests (`CalculateApiIntegrationTests.cs`)  
Validates the full **API pipeline** (request → deserialization → calculation → response):  

| Test Method | Purpose |
|-------------|---------|
| `PostJsonRequest_ReturnsJsonResponse` | Confirms JSON requests are deserialized, processed recursively, and results returned as **application/json**. Verifies correct result (`25`). |
| `PostXmlRequest_ReturnsXmlResponse` | Confirms XML requests are deserialized, processed recursively, and results returned as **application/xml**. Ensures `<Result>` element is present in response. |

---

✅ Together, these tests cover:  
- **Core calculation logic** (service).  
- **Recursive evaluation of nested operations**.  
- **Error handling** for unsupported operations and empty values.  
- **API correctness** (mocked service + full request/response).  
- **Format handling** (JSON and XML).  

---
### ▶️ How to Run

📥 Clone the repository:
```
git clone https://github.com/angelkomarov/Calculator.Api.git
```

📂 Navigate to the project folder:
```
cd Calculator.Api
```

⚡ Build and run the API:
```
dotnet run
```

🧪 Send a POST request with either XML or JSON input using Postman or curl.
