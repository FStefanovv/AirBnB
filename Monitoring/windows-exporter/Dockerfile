FROM mcr.microsoft.com/windows/nanoserver

ENV PATH="C:\Windows\system32;C:\Windows;"
COPY output/amd64/windows_exporter.exe /windows_exporter.exe
ENTRYPOINT ["windows_exporter.exe"]