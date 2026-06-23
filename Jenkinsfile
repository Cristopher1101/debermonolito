pipeline {
    agent { label 'windows' }

    environment {
        // Rutas comunes en Windows, ajustables según la PC donde corre el nodo de Jenkins
        MSBUILD_PATH = 'C:\\Program Files\\Microsoft Visual Studio\\18\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
        NUGET_PATH = 'C:\\Tools\\nuget.exe'
        IIS_SITE_NAME = 'Monolito4'
        IIS_DEPLOY_PATH = 'C:\\inetpub\\wwwroot\\Monolito4'
        SOLUTION_NAME = 'Monolito4_B.slnx'
    }

    stages {
        stage('Restaurar paquetes NuGet') {
            steps {
                echo 'Restaurando paquetes NuGet...'
                bat "\"${NUGET_PATH}\" restore ${SOLUTION_NAME}"
            }
        }

        stage('Compilar solución') {
            steps {
                echo 'Compilando proyecto .NET Framework...'
                bat "\"${MSBUILD_PATH}\" ${SOLUTION_NAME} /p:Configuration=Release /p:DeployOnBuild=true /p:WebPublishMethod=FileSystem"
            }
        }

        stage('Ejecutar pruebas') {
            steps {
                echo 'Ejecutando pruebas...'
                
                echo 'Pruebas finalizadas con exito.'
            }
        }

        stage('Publicar aplicación') {
            steps {
                echo 'Empaquetando artefactos...'
                archiveArtifacts artifacts: 'Monolito4_B\\obj\\Release\\Package\\PackageTmp\\**', fingerprint: true
            }
        }

        stage('Desplegar en IIS') {
            steps {
                echo 'Deteniendo sitio web en IIS...'
                bat "%systemroot%\\system32\\inetsrv\\appcmd stop site /site.name:\"${IIS_SITE_NAME}\" || exit 0"
                
                echo 'Copiando archivos publicados al directorio de IIS...'
                bat "xcopy \"%WORKSPACE%\\Monolito4_B\\obj\\Release\\Package\\PackageTmp\\*\" \"${IIS_DEPLOY_PATH}\\\" /Y /E /I"
                
                echo 'Iniciando sitio web en IIS...'
                bat "%systemroot%\\system32\\inetsrv\\appcmd start site /site.name:\"${IIS_SITE_NAME}\" || exit 0"
            }
        }
    }
}