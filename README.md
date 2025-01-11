# Catedra3Backend

# Prerequisitos
Debe tener instalado: .Net SDK, SQL Server y Visual Studio Code

# Clonación y ejecución
Primero clonamos en consola el siguiente repositorio:
git clone https://github.com/Damatu0348/Catedra3Backend.git

Luego navegamos a la carpeta clave:
cd Catedra3Backend

Después restauramos dependencias de NuGet :
dotnet restore

Abrir en Visual Studio code:
code .

(Opcional) Configurar base de datos:
dotnet ef database update

Ejecutar aplicación con:
dotnet run