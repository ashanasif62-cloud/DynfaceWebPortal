# API CONSUMING CLASS PROTOCOL - VERSION 1.4

## 1. MANDATORY: Execution Sequence
This protocol must be followed in a strict, one-by-one sequence. Do not proceed to the next step until the current one is satisfied.

### Step 1: Class Identity
**Ask the user:** "What is the name of the **Consuming Class**?"

### Step 2: Service Mapping
**Ask the user:** "What is the name of the **Connected Service**?"

### Step 3: Method Discovery & Verification
1. **Internal Search:** Search the workspace for the exact API method required.
2. **External Fallback (Mandatory):** If the method is not found internally, **STOP** and ask: "I couldn't find the exact method in the workspace. Can you share the **URL or documentation link** for this endpoint?"
3. **Extraction Logic:** Upon receiving a link, analyze the documentation (WSDL/Swagger/Docs) to extract the method signature, required parameters, and return types.

### Step 4: Generation & Architectural Standards
Once the method details are confirmed, generate the class according to these architectural guardrails:

* **Namespace:** Place the class strictly within the `PortalIntegration` namespace.
* **Service Constants:** Define a private `serviceName` (e.g., `...SvcGroup`) and a public `tableName`.
* **Connectivity Pattern:** Exclusively use the `OperationContextScope`, `SoapHelper`, and `OAuthHelper.getAuthenticationHeader()` pattern for all service calls.
* **Context Handling:** Initialize `CallContext` with `Guid.NewGuid()` and `dataAreaId` from `SessionVariables`.
* **Error Handling:** Every method must include a `try-catch-finally` block. The `catch` must use `SysErrorLog` and `System.Reflection.MethodBase` to log the full method name and the exception.
* **Data Conversion:** Methods returning `DataTable` must use `RetrieveDatatable.createDataTable(results)`.
* **Helper Methods:** Include a `createDataTable()` helper method at the end of the class to initialize empty tables with the correct schema.

---

### **Final Compliance Check**
* Did you ask for the class and service names individually?
* Is the namespace `PortalIntegration` (not DataAccess)?
* Does the code implement the `using (OperationContextScope operationContextScope = new OperationContextScope(channel))` block?
* Are `OAuthHelper` and `SoapHelper` patterns applied correctly?
* Does the error log capture the `DeclaringType.FullName`?