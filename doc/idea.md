# MarkIt

- [MarkIt](#markit)
    - [Unsere Idee](#unsere-idee)
    - [Must-Haves](#must-haves)
    - [Nice-To-Haves](#nice-to-haves)
    - [Aufgabenteilung (circa)](#aufgabenteilung-circa)
    - [Cloud-Syncs/Work-Togheter](#cloud-syncswork-togheter)
    - [User-Accounts](#user-accounts)
    - [Skizzen](#skizzen)
    - [Klassen-UML](#klassen-uml)

### Unsere Idee

**MarkIt** ist ein Texteditor für Markdown-Dateien. Als Grundprinzip orientieren wir uns an **MS Word**. Man kann ganz normal Dateien öffnen, schreiben und wieder speichern.
Man kann außerdem die **Basic-Markdown-Syntax** anwenden: Wenn man zum Beispiel auf einen Button klickt, wird der Text zu `**bold**`.

### Must-Haves

* UI wie ein Texteditor (mit einem Feld zum Schreiben, ...)
* Lesen von Markdown-, Text- und JSON-Dateien
* Schreiben von Markdown-, Text- und JSON-Dateien
* Basic-Settings (Farbschema, Bildgröße, Textgröße, Speicherpfade, ...)
* Datei-Verlauf: Welche Dateien wurden zuletzt geöffnet/verwendet?

### Nice-To-Haves

* Verschiedene User-Accounts
* Work-Together
* 2-Faktor-Authentifizierung
* Bilder importieren (wie in VS Code)
* Cloud-Sync
* Real-Time-Rendering

### Aufgabenteilung (circa)

| Karim                      | Max                     |
| -------------------------- | ----------------------- |
| Settings                   | Cloud-Sync              |
| Lesen von Dateien          | Dokumentation & Planung |
| Schreiben von Dateien      | Datei-Verlauf           |
| WPF-UI mit Backend         | WPF-UI mit Backend      |
| 2-Faktor-Authentifizierung | User-Profile            |
| Work-Together              | Work-Together           |
| Real-Time-Rendering        | Real-Time-Rendering     |

### Cloud-Syncs/Work-Togheter

Mit einem eigenen physischen Server, auf dem Ubuntu Server läuft, wollen wir im Code alle Funktionen mit dem Server verbinden. Bei Cloud-Saves verbindet sich die App mit dem Server und lädt die Dateien hoch bzw. herunter.

### User-Accounts

User-Accounts wollen wir entweder mit einer kleinen SQL-Datenbank verwalten oder eine Datei auf dem Server erstellen, in der die Accounts und Passwörter gespeichert sind (das ist zeitlich realistischer).

### Skizzen

![alt text](images/main-window.png)
![alt text](images/settings.png)

### Klassen-UML

@startuml
top to bottom direction

skinparam classAttributeIconSize 0

class UserManager {
    + ErrorType : enum
    --
    + UserManager()
    --
    + SignInAndHandleErrors(email : string, password : string): void
    + SignUpAndHandleErrors(email: string, password: string): void
    + GetRememberedUsers(): List<Session>
    + WriteToRememberedUsers(currentSession: session): void
}

class Logger {
    + Logger : ILogger
    --
    + Init(): void
}

class ServerSettings {
    <u>+ Port: int {get; private set}
    <u>+ PublicIP: string {get; private set}
    <u>+ Username: string {get; private set}
    <u>+ KeyFilePath: string {get; private set}
    --
    <u>+ Init(): void
}

class ServerManager{
    + ServerStatus: enum
    --
    + ServerManager()
    --
    + InitSupabaseClient(): void
    + GetStatus(): ServerStatus
}

class ClassUser{
    + Email: string {get ; set}
    + Password: string {get ; set}
    --
    + ClassUser(email: string, password:string)
}

' ChatGPT Anfang
' prompt: wie kann ich in plantuml diagramme untereinenader machen
UserManager -[hidden]-> Logger
Logger -[hidden]-> ServerSettings
ServerSettings -[hidden]-> ServerManager
ServerManager -[hidden]-> ClassUser
' ChatGPT ende

@enduml