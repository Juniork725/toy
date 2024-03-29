## NanaCandy
Unity에 흥미가 생겨서 며칠간 공부를 했다. 처음 다뤄보는 툴이라 숙련도가 없다보니 세세한 부분들이 예상대로 안 되거나 원하는 기능을 어떻게 구현해야 하는지 몰라서 많이 막혔다.  
그래도 UI가 다루기 쉽게 되어있고 워낙 관련 자료가 많다보니 하나씩 찾아가며 일단 하나의 게임으로 완성했다.  

게임 이름은 NanaCandy이고, 장르는 퍼즐이다. 서로 분리된 2개의 필드에 캐릭터가 하나씩 존재하는데, 방향키로 조작하면 두 캐릭터가 서로 반대 방향으로 움직인다. 두 캐릭터가 동시에 사탕에 도달하면 스테이지가 클리어 되는 구조다.  
캐릭터의 진행을 막는 벽, 밟으면 처음 위치로 돌아가는 함정, 들어가면 반대쪽으로 나오는 포탈 등을 추가해 난이도를 조절했다.  

프로젝트 전체를 업로드 하고 싶었는데, LFS로 올려보려다가 무료 용량 제한 때문인지 업로드가 안 되어서 주요 코드 파일만 업로드했다.  
cs 파일은 프로젝트에 사용된 scripts이고 tester.py는 게임에서 제작한 스테이지의 최단 경로를 찾기 위한 코드다.  

작업을 시작하기 전에는 캐릭터 디자인이나 음향 효과 등을 어떻게 해결할지 좀 걱정했는데, 생각보다 오픈소스가 많기도 하고 2D 도트 그래픽을 간단하게 만들 수 있는 사이트가 있어서 쉽게 해결했다.  
역시 뭐든 고민만 하는 것보다 일단 도전해보고 하나씩 해결해나가는 게 좋은 방법 같다.  

그리고 게임을 만들 때 실행시켜보고 막히는 부분마다 코드를 수정하는 식으로 해서 잘 몰랐는데, 리뷰를 하다 보니 필요없는 코드가 남아있거나 논리적으로 하나의 과정이 여러 메소드에 쪼개져있거나 하는 경우가 있었다.  
이번에는 혼자 만든 작은 프로젝트라 별 문제가 없었지만 여러 명이 협업하는 큰 프로젝트에서는 이런 부분들이 코드를 개선하는 데에 어려움을 줄 듯하다. 당장 프로그램이 멀쩡하게 돌아가는 것에 그치지 않고 전체적인 흐름을 생각하면서 깔끔한 코드를 작성할 수 있도록 노력해야겠다.  


### 플레이 영상
![NanaCandy Playing Video](https://user-images.githubusercontent.com/62535139/229797755-287615ea-a82a-4802-8aa7-3b9f31e14a51.gif)
