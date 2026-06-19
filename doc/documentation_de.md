# MarkIt Dokumentation (Plichtenheft)

- [MarkIt Dokumentation (Plichtenheft)](#markit-dokumentation-plichtenheft)
  - [Anforderungen und Versionen](#anforderungen-und-versionen)
  - [Implementierungsbeschreibung](#implementierungsbeschreibung)

## Anforderungen und Versionen

MarkIt läuft nur unter Windows. Es müssen **keine weiteren Anforderungen oder Programme installiert werden**.

## Implementierungsbeschreibung

Die Entwicklung unseres Markdown-Editors begann mit der Planungsphase. In dieser Phase besprachen wir die Funktionen, die wir implementieren wollten, erstellten eine grobe Projektstruktur und entschieden uns für die Technologien, die wir verwenden würden.

Nachdem die Planung abgeschlossen war, begannen wir mit der Implementierung des Login- und Registrierungssystems. Dadurch konnten Benutzer Konten erstellen und sich sicher in der Anwendung anmelden. Die Benutzerdaten und die Authentifizierung wurden über Supabase verwaltet.

Sobald das Authentifizierungssystem funktionierte, konzentrierten wir uns auf die Benutzeroberfläche. Wir gestalteten das Layout des Editors, erstellten Menüs und Einstellungsseiten und verbesserten das allgemeine Erscheinungsbild der Anwendung, um sie benutzerfreundlicher und optisch ansprechender zu machen.

Der nächste Schritt war die Implementierung der Kernfunktionen des Editors. Wir fügten die Möglichkeit hinzu, Markdown-Text zu schreiben sowie Dokumente zu speichern und zu laden. Dadurch konnten Benutzer ihre Arbeit speichern und später weiterbearbeiten.

Nachdem die grundlegenden Funktionen des Editors fertiggestellt waren, implementierten wir das Live-Rendering der Markdown-Elemente. Diese Funktion zeigt die formatierte Ausgabe in Echtzeit an, während der Benutzer schreibt, wodurch der Bearbeitungsprozess komfortabler und interaktiver wird.

Für die Backend-Infrastruktur verwendeten wir Supabase. Zu Beginn des Projekts wurde der Supabase-Server auf einem Server bei unseren Großeltern gehostet, da dort Portweiterleitungen (Port Forwarding) möglich waren. Diese Lösung erwies sich jedoch als nicht besonders zuverlässig, da der Server nicht immer eingeschaltet war und gelegentlich Verbindungsprobleme verursachte.

Um die Zuverlässigkeit zu verbessern, verlagerten wir den Server in unsere lokale Umgebung. Anschließend nutzten wir Cloudflare Tunnels, um den Server über das Internet erreichbar zu machen. Da wir die kostenlose Version von Cloudflare Tunnels verwendeten, änderte sich die generierte Domain regelmäßig.

Um dieses Problem zu lösen, entwickelten wir ein System, das die aktuelle Tunnel-Domain automatisch in einem GitHub Gist aktualisierte. Die Client-Anwendung rief diese Domain anschließend über die GitHub-Gist-API ab, wenn eine Verbindung zum Server hergestellt werden musste. Dadurch konnten Benutzer den Backend-Server immer erreichen, auch wenn sich die Tunnel-Adresse geändert hatte.

Nachdem alle wichtigen Funktionen implementiert waren, konzentrierten wir uns auf Fehlerbehebungen, die Verbesserung der Stabilität und die Überarbeitung der Dokumentation. Nach abschließenden Tests und Kontrollen wurde das Projekt für die Endabgabe vorbereitet.
