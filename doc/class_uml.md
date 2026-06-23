### Klassen-UML

@startuml
top to bottom direction
skinparam classAttributeIconSize 0
skinparam padding 1
skinparam nodesep 60
skinparam ranksep 60

class UserManager {
    + errorType : ErrorType
    --
    + UserManager()
    --
    + SignInAndHandleErrors(email : string, password : string): void
    + SignUpAndHandleErrors(email: string, password: string): void
    + GetRememberedUsers(): List<Session>
    + WriteToRememberedUsers(currentSession: session): void
}

enum ErrorType{
    Unknown
    OK
    ServerUnreachable
    PasswordFalse
    BadPassword
    BadEmail
    EmailExists
    Requests
    Confirmation
}

UserManager ..> ErrorType

class Logger {
    + Logger : ILogger
    --
    + Init(): void
}

class ServerSettings {
    <u>+ URL: string {get; private set}
    --
    <u>+ Init(): void
}

class ServerManager{
    + serverStatus: ServerStatus
    --
    + ServerManager()
    --
    + InitSupabaseClient(): void
    + GetStatus(): ServerStatus
}

enum ServerStatus{
    On
    Off
    Unknown
}

ServerManager ..> ServerSettings
ServerManager ..> ServerStatus

class ClassUser{
    + Email: string {get ; set}
    + Password: string {get ; set}
    --
    + ClassUser(email: string, password:string)
}

class GeneralSettings {
    + width: double { get; set; }
    + height: double { get; set; }
    + iconAnimations: bool { get; set; }
    + liveRendering: bool { get; set; }
    + animationFPS: string { get; set; }
    + updatedColorTheme: bool { get; set; }
    + currentColorTheme: ColorTheme? { get; set; }
    - colorThemes: List<ColorTheme>? { get; set; }

    --
    + GeneralSettings()
    + GeneralSettings(width: double, height: double, iconAnimation:bool liveRendering: bool, animationFPS: string)

    --
    + LoadFromFile(filename: string): GeneralSettings?
    + SaveToFile(filename: string): void
    + GetAllColors(): List<ColorTheme>?
    + ChangeColor(color: ColorTheme): void
    + ChangeSize(width: double, height: double): void
    + SaveColorsToFile(): void
    - setColorsFromFile(): void
}

class ColorTheme{
    + Name: string {get; init}
    + HoverColor: string {get; init}
    + BackgroundColor: string {get; init}
    + Foreground: string {get; init}
    + TextColor: string {get; init}
    --
    + ColorTheme(name: string, hovercolor: string. backgroundcolor: string, foreground:string, textcolor: string)
}

GeneralSettings ..> ColorTheme

class ClassWorksheet {
    - wsName: string = "markdown"
    - wsCreationDate: DateTime
    - wsStringPages: List<string>
    - wsWidth: double
    + wsHeight: double
    + gridWorkSheet: Grid
    - canvisWsPages: List<Canvas>
    - stackpanelWorksheet: StackPanel
    - Zoom: double
    - pageMargin: double
    - AddingInProzess: bool
    + ScrollViewerWorksheet: ScrollViewer { get; private set; }

    --
    + ClassWorksheet()
    + ClassWorksheet(gridWorkSheet: Grid)

    --
    + RenderLines(): void
    + Render(): void
    + addToPostion(symbols: string): void
    + addToPostion(symbols: string, symbolsEND: string): void
    + addToLineBeginning(symbol: string): void
    + addTabel(height: int, width: int): void
    + LoadFromString(content: string): void
    + GetContent(): string

    --
    - CT_TextBox_PreviewKeyDown(sender: object, e: KeyEventArgs): void
    - CT_TextBox_TextChanged(sender: object, e: TextChangedEventArgs): void
    - checkCurrentLine(): int
    - getCursorPosition(line: int): int
    - moveLineUp(): void
    - moveLineDown(): void
    - makeLineNoCarterIDX(lineContent: string): void
    - addLine(): void
    - deletLine(): void
}

class Page {
    - content: string
    - page: Grid
    - height: double
    - width: double

    --
    + Page()
    + Page(content: string)

    --
    + Render(): Grid
    + SplitToTextboxes(): void
    + locateFormat(): List<List<int>>
    + ToString(): string
}


class WindowMessageBox{
    + returnType: ReturnType {get ; private set}
    + buttonType: ButtonType {get ; private set}
    --
    + WindowMessageBox(heading: string, content: string)
    + WindowMessageBox(heading: string, content: string, buttonType: ButtonType)
    --
    - DrawButtons(buttonType: ButtonType): void
}

enum ReturnType{
    Okay
    Yes
    No
}

enum ButtonType{
    Okay
    Yes
    No
}

WindowMessageBox ..> ReturnType
WindowMessageBox ..> ButtonType


class FileManager {
    - userEmail: string
    + userPath: string
    + FileHistory: List<FileHistoryItem>
    + CurrentFilePath: string?

    --
    + FileManager(userEmail: string)

    --
    + SaveToFile(path: string, content: string): void
    + LoadFromFile(path: string): string?
    + AddToHistory(item: FileHistoryItem): void
    + CreateNewFile(): bool
    + Upload(path: string, content: string): Task<bool>
    + Download(path: string): Task<string>
    + MarkdownToPdf(markdown: string): Task

    --
    - createUserFolder(): void
    - saveHistory(): void
}

' ChatGPT Anfang
' prompt: wie kann ich in plantuml diagramme untereinenader machen
UserManager -[hidden]-> Logger
Logger -[hidden]-> ServerSettings
ServerSettings -[hidden]-> ServerManager
ServerManager -[hidden]-> ClassUser
' ChatGPT ende


@enduml