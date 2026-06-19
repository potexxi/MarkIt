# MarkIt Documentation (Plichtenheft)

- [MarkIt Documentation (Plichtenheft)](#markit-documentation-plichtenheft)
  - [Requirements and Versions](#requirements-and-versions)
  - [Implementation description](#implementation-description)

## Requirements and Versions

MarkIt only runs on Windows. You **do not** need to install further requirements.

## Implementation description

The development of our Markdown Editor started with the planning phase. In this phase, we discussed the features we wanted to implement, created a rough project structure, and decided which technologies we would use.

After the planning was finished, we started implementing the login and registration system. This allowed users to create accounts and securely log into the application. User data and authentication were handled through Supabase.

Once the authentication system was working, we focused on the user interface. We designed the editor layout, created menus and settings pages, and improved the overall appearance of the application to make it more user-friendly and visually appealing.

The next step was implementing the core editor functionality. We added the ability to write Markdown text as well as save and load documents. This allowed users to store their work and continue editing it later.

After the basic editor features were completed, we implemented live rendering for Markdown elements. This feature displays the formatted output in real time while the user is typing, making the editing process more convenient and interactive.

For the backend infrastructure, we used Supabase. At the beginning of the project, the Supabase server was hosted on a server located at our grandparents' house because port forwarding was available there. However, this setup was not very reliable because the server was not always running and occasionally caused connection problems.

To improve reliability, we moved the server to our local environment. We then used Cloudflare Tunnels to make the server accessible from the internet. Since we used the free version of Cloudflare Tunnels, the generated domain changed regularly.

To solve this issue, we created a system that automatically updated the current tunnel domain in a GitHub Gist. The client application then retrieved the latest domain through the GitHub Gist API whenever it needed to connect to the server. This ensured that users could always reach the backend, even when the tunnel address changed.

After all major features were implemented, we focused on fixing bugs, improving stability, and updating the documentation. Once everything was tested and reviewed, the project was prepared for the final submission.

