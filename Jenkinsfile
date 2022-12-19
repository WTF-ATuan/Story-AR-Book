pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo "Clean workspace. (${WORK_SPACE})"
        dir(path: "${WORK_SPACE}") {
          bat 'git clean -f -d -x -e /[Ll]ibrary/ /[Pp]ackages/'
        }

        bat 'curl -X POST https://api.telegram.org/bot$TELEGRAM_BOT_TOKEN/sendMessage -d parse_mode=markdown -d chat_id=987110561 -d text='+"\"Build pipeline starts! - *${JOB_NAME} #${BUILD_NUMBER}* - ([Go to Jenkins](${BUILD_URL}))\""
      }
    }

    stage('Build & Analyze') {
      parallel {
        stage('Build Android') {
          environment {
            SYMBOL_CONFIG = 'Release'
            BUILD_TARGET = 'Android'
            UNITY_BUILD_METHOD = 'Core.Project.ProjectBuilder.BuildProject'
          }
          steps {
            echo "Build ${SYMBOL_CONFIG} ${BUILD_TARGET} with Unity (${UNITY_PATH})"
            echo "Project path: ${UNITY_PROJECT_DIR}"
            echo "Output path: ${OUTPUT_PATH}"
            bat "${UNITY_PATH} -projectPath ${UNITY_PROJECT_DIR} -buildTarget ${BUILD_TARGET} -executeMethod ${UNITY_BUILD_METHOD} -logFile - -quit -batchmode -nographics -outputPath ${OUTPUT_PATH} -defineSymbolConfig ${SYMBOL_CONFIG}"
          }
        }

      }
    }

    stage('Deploy') {
      steps {
        archiveArtifacts 'Artifacts/**'
      }
    }

  }
  environment {
    WORK_SPACE = "${WORKSPACE}".replace("\\", "/")
    OUTPUT_PATH = "${WORK_SPACE}/Artifacts"
    UNITY_PATH = '"C:/Program Files/Unity/Hub/Editor/2022.2.0f1/Editor/Unity.exe"'
    UNITY_PROJECT_DIR = "${WORK_SPACE}"
  }
}