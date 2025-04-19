# Banking-System

This is a secure Banking System API with role-based access, multi-branch support, and transaction features.  
Final project at InMind Academy, which led to my selection for an internship in Germany due to excellent performance.

---

##  Project Description

A full-featured Banking System API designed to apply all the knowledge acquired during the training module at InMind Academy.

---

##  Objective

Build a complete and secure API that simulates a real-world banking system, with proper data access control and business logic.

---

##  Key Features

- **Multi-branch architecture**
  - Each branch can only view its own data.

- **Customer operations**
  - Customers can view their accounts and perform transactions (withdrawals & deposits).
  - Each customer can have up to **5 accounts** across all branches.

- **Employee operations**
  - Employees can create accounts for customers.
  - Can add **recurrent transactions** to client accounts.
  - Have **write access** only within their branch, and **read access** to others.

- **User roles and permissions**
  - **Admin**: Full access to all data across the system.
  - **Employee**: Limited to their branch (write) and read access elsewhere.
  - **Customer**: Can only access their own accounts and transactions.

---