# 첫번째 코드
```python
#상위 30% crossover

import random
from matplotlib import pyplot as plt

def RSP(env,life):
    if (env=='R' and life=='P') or (env=='S' and life=='R') or (env=='P' and life=='S'):
        return 'win'
    elif (env=='R' and life=='S') or (env=='S' and life=='P') or (env=='P' and life=='R'):
        return 'lose'
    else:
        return 'draw'
    
def main():
    p=eval(input("바위의 확률:"))
    l=eval(input("data set 길이:"))
    num=eval(input("life data set 개수:"))
    gen=eval(input("진행할 세대 수:"))
    mut=eval(input("mutation rate:"))
    #parameter 설정
    
    prob=[]
    for i in range(100):
        if i<p:
            prob.append('R')
        else:
            prob.append('S')
    #확률 적용
            
    normal = ['R','S','P']
    #R=바위, S=가위, P=보

    life=[]
    for i in range(num):
        temp=[]
        for j in range(l):
            temp.append(random.choice(normal))
        life.append(temp)
    #최초 life data set 구성

    avg=[]
    top_avg=[]
    process=0
    for i in range(gen+1):
        env=[]
        scr={}
        for j in range(l):
            env.append(random.choice(prob))
            #해당 세대의 env 형성
        temp=0
        for j in life:
            score=0
            for k in range(l):
                result=RSP(env[k],j[k])
                if result=='win':
                    score+=1
                elif result=='lose':
                    score-=1
            j.append(score/l)
            #scoring
            temp+=score/l
            if j[-1] in scr:
                scr[j[-1]].append(j)
            else:
                scr[j[-1]]=[j]
        avg.append(temp/num)
        #average of score
        top_num=0
        temp=0
        top=[]
        scr_list=[]
        for j in scr:
            scr_list.append(j)
        scr_list.sort(reverse=True)
        for j in scr_list:
            n=len(scr[j])
            top_num+=n
            temp+=n*j
            top+=scr[j]
            if top_num>=num/30:
                break
        top_avg.append(temp/top_num)
        #average of score of top 30%

        locus=[]    #gene pool for crossover
        for j in range(int(l/10)+int(not l%10==0)):
            locus.append([])
        for j in top:
            k=0
            while k*10<l:
                locus[k].append(j[10*k:10*(k+1)])
                k+=1
        next_life=[]
        for j in range(num):
            temp=[]
            for k in locus:
                temp+=random.choice(k)
            next_life.append(temp)
        #making next generation by crossover

        life=next_life

        mut_num=int(l*mut)
        for j in life:
            mut_site=random.sample(range(l),mut_num)
            for k in mut_site:
                j[k]=random.choice(normal)
        #give mutation to next generation by mutation rate
        
        if i/gen*100>=process:
            print(str(process)+'%')
            process+=10
        #show the percentage of processing
    
    plt.plot(avg)
    plt.xlabel('Generation')
    plt.ylabel('Fitness')
    plt.title('Average')
    plt.ylim(0.0,1.0)
    plt.xlim(0,gen)
    plt.show()
    plt.plot(top_avg)
    plt.xlabel('Generation')
    plt.ylabel('Fitness')
    plt.ylim(0.0,1.0)
    plt.xlim(0,gen)
    plt.title('Average of top 30%')
    plt.show()

main()
```
유전 알고리즘의 원리와 진화 이론을 이용해서 기초적인 학습 모델을 구현해봤다.  
가위바위보를 응용해서 학습 환경과 목표를 설정했다. 가위/바위가 랜덤하게 존재하는 환경에서 각 개체는 랜덤하게 가위,바위,보를 낸다.  
개체들은 서로 동일한 환경 속에서 승리한 횟수로 평가받고, 높은 평가를 받은 개체들을 위주로 다음 세대를 구성하는 것이 기본 원리다.  

바위의 확률을 0, 개체를 평가할 환경인 data set의 길이를 100, 한 세대를 구성하는 개체의 수를 100, 학습을 진행할 세대 수를 100, 각 세대에서 mutation이 일어날 확률을 0.01로 설정했을 때, 아래 그림과 같은 결과 그래프가 나왔다.  
![Figure_1](https://user-images.githubusercontent.com/62535139/212324854-d734a373-a63b-4d93-af47-594e4002137a.png)
![Figure_2](https://user-images.githubusercontent.com/62535139/212325646-078cf617-17f9-457a-a125-fc12d64b9ae7.png)  
대략 30세대 정도만에 환경에 적응을 마쳤다.  

위 조건에서 바위의 확률만 0.5로 조정한 결과는 다음과 같다.  
![Figure_3](https://user-images.githubusercontent.com/62535139/212325651-b1f6cc58-422c-454d-b82b-c3499aad5814.png)
![Figure_4](https://user-images.githubusercontent.com/62535139/212325664-e9605da3-0aa6-4562-b1c5-707cd7311938.png)  
이전 경우와 달리 fitness가 조금 진동하긴 하지만 이번에도 비슷하게 40세대 정도에 적응을 마친 셈이다.  

이번 코드는 내가 유전 알고리즘을 실제로 구현할 수 있을까 하는 생각으로 도전해 본 결과물이다. 이 코드를 짜면서 데이터의 흐름에 대한 감이 조금 잡혀서 본격적으로 학습 모델을 구현해봤다.  
그 결과물 곧 2nd.md, 3rd.md로 작성할 예정.
