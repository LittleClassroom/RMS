Secure credentials and how RMS stores DB connection strings

Overview

This document explains how RMS encrypts and stores database connection credentials and how the UI interacts with that storage.

Where credentials are stored

- App default (per current UI behavior): `db.creds` placed in the project root (detected by searching upward for a `.csproj`).
- Alternative per-user storage (previous behavior / recommended for production): `%APPDATA%\CSharpAssignment\db.creds` (see `Global.GetCredentialFilePath()`).
- Export/import file (optional): you can export an encrypted connection as a `.conn` file using the Save/Open dialogs in the connection UI.

Encryption format

- The project uses AES?GCM for authenticated encryption implemented in `RMS.Utils.SecureConfig`.
- File layout: `[salt(16)][nonce(12)][tag(16)][ciphertext(...)]`.
- Key derivation: PBKDF2 (Rfc2898DeriveBytes) with SHA?256, 100,000 iterations, 32?byte key length.
- By default the code uses a built?in static secret `AppSecret` (in `SecureConfig`) so no user passphrase is required.
- Optionally you may call the overloads that accept a user passphrase. When using a passphrase, the same passphrase must be supplied to decrypt the file later.

How the app reads/writes credentials

- Save flow (example): `SecureConfig.SaveEncryptedConnectionString(connectionString, path)`
  - Generates random salt and nonce
  - Derives key with PBKDF2(passphrase or `AppSecret`, salt)
  - Encrypts with AES?GCM and writes the file
- Load flow: `SecureConfig.LoadEncryptedConnectionString(path)`
  - Reads the file, extracts salt/nonce/tag/ciphertext
  - Derives the same key and decrypts with AES?GCM
  - Returns the plaintext connection string
- After load, the UI calls `RMS.Global.SetConnection(connectionString)` so the application uses the new connection immediately.

Notes and recommendations

- Development vs production:
  - Saving `db.creds` to the project folder is convenient for development but not recommended for production (writing to `Program Files` requires elevation and the file may be shared among users).
  - For deployed apps, prefer the per?user path returned by `Global.GetCredentialFilePath()` or a secured store.

- Security tradeoffs:
  - Using `AppSecret` (static secret in the binary) is convenient but weaker than requiring a user passphrase. If an attacker can read the binary they may recover `AppSecret`.
  - Requiring a user passphrase is more secure but makes recovery impossible if forgotten.

- Backup and migration:
  - The `.conn` export is the same encrypted blob format and can be used to move credentials between machines (passphrase required if used).

- Where to change behavior:
  - `Utils/SecureConfig.cs` — change key derivation params or `AppSecret`.
  - `UI/DbCredentialsForm.cs` — controls whether save/load uses the project folder, `%APPDATA%`, or shows export dialogs.
  - `Global` helpers (`Global.SaveAndPersistConnection`, `CredentialStore`) — integration with app storage and DPAPI fallbacks.

Troubleshooting

- If decrypt fails with a `CryptographicException`:
  - Ensure you used the same passphrase (if one was used) or that `AppSecret` in the binary matches the one that encrypted the file.
  - Verify the file wasn’t corrupted/truncated.

Questions or changes

If you want a different default (always use `%APPDATA%`, require passphrase, or add an export/import button), tell me which behavior and I will update the UI code accordingly.
