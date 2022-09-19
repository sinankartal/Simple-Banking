# Simple-Banking

Simple Banking is an API for making basic banking operations by the system admin. (Create Account, Account Top Up, Money Transfer)

## Configuration
- The latest version of .NET 6 should be installed.

## Assumptions

- There is only one user in the system and he/she is an admin of the system.
- Admin can make basic operations.
- IBANs are stored in the database in advance.
- Transaction fees and limits are created in the system in advance.

## Controller Endpoints

- "api/User/login": Performs user authentication and returns a bearer token. (In order to receive the token use username:'admin', password:'admin')
- "api/Account": Creates new user account.
- "/api/Account/{id}": Gets the account information by id.
- "/api/Account/{bsn}": Gets the account information by BSN.

- "api/FinancialOperation/topup": Puts money to bank account after curring fees
- "api/FinancialOperation/money-transfer": Transfers money from given account to another account by IBAn and user information.

## Transaction Types

All account activities are logged in the system after the successful operation.

- MONEY_TRANSFER: Type of log after money transfer
- TOPUP: Type of log after topup,
- FEE: Type of log after fee cutted.

## Account Activity Types
There is two different types of account activities in the system currently.
- MONEY_TRANSFER: Money transfer
- TOPUP: Account balance top up

## Execution
- Run the API project under the "src" folder.
