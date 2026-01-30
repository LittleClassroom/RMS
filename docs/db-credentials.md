# Database Credential System

## Purpose
Store database connection credentials securely so the application can connect without exposing plaintext secrets in source control or configuration files.

## How it works (implementation)
- Credentials are gathered via the `DbCredentialsForm` UI (`UI/DbCredentialsForm.cs`).
- The connection string is serialized to JSON and encrypted using Windows DPAPI (`ProtectedData`) with `DataProtectionScope.CurrentUser`.
- Encrypted bytes are written to `%APPDATA%/CSharpAssignment/db.creds` by `Utils/CredentialStore`.
- On startup `Global.LoadFromStore()` attempts to read and decrypt the file and initialize `Global.SqlCon`.

## File location and format
- File: `%APPDATA%/CSharpAssignment/db.creds`
- Content: encrypted binary blob (not human-readable). Original payload is JSON: `{ "ConnectionString": "..." }`.

## Encryption
- Uses Windows DPAPI via `System.Security.Cryptography.ProtectedData.Protect` and `Unprotect`.
- Entropy: fixed app byte array (`CSharpAssignmentEntropy_v1`) to bind the protection to this app version.
- Scope: `CurrentUser` — only the same Windows user can decrypt the file on the same machine.

## Usage
- First run: application prompts for DB credentials. Enter Server, Database, User ID, and Password and press `Test` then `Save`.
- On successful save the app stores encrypted credentials and initializes the DB connection.
- If the user cancels the credentials dialog the application exits.

## Management
- To clear stored credentials programmatically call `Global.ClearStoredCredentials()` or `Utils.CredentialStore.ClearCredentials()`.
- The UI can be extended to include a settings page to update or clear credentials.

## Security considerations
- DPAPI `CurrentUser` scope ties the blob to the OS user; it is not portable across machines or users.
- For multi-user or cloud scenarios use a centralized secret store (Azure Key Vault, HashiCorp Vault) or an enterprise key management solution.
- Do not commit `db.creds` or any `*.creds` files to source control. `.gitignore` includes patterns to prevent this.

## Extending or changing storage
- To change file path: update `Utils/CredentialStore.AppFolder` and `CredFile`.
- To change protection: replace DPAPI calls with a different crypto provider (ensure key storage is secure).

## Troubleshooting
- "Unable to decrypt": likely different user or corruption; remove the file and re-enter credentials.
- "Connection test failed": check server, credentials, and network. Use `DbCredentialsForm` Test button.

## References
- `Utils/CredentialStore.cs` — encryption and file IO
- `UI/DbCredentialsForm.cs` — credential UI
- `Global.cs` — load/test/clear helpers
