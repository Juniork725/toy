from collections import deque

def main(mapA, mapB, starR1, starC1, starR2, starC2):
    len_r, len_c = len(mapA), len(mapA[0])
    map_memo = []

    portalA1, portalA2, portalB1, portalB2 = None, None, None, None
    
    for r in range(len_r):
        mapA_r = []
        for c in range(len_c):
            mapA_c = []

            if mapA[r][c] == 4:
                portalA1 = (r,c)
            elif mapA[r][c] == 5:
                portalA2 = (r,c)

            if mapB[r][c] == 4:
                portalB1 = (r,c)
            elif mapB[r][c] == 5:
                portalB2 = (r,c)

            for _ in range(len_r):
                mapA_c.append([9999]*len_c)
            mapA_r.append(mapA_c)
        map_memo.append(mapA_r)
    map_memo[starR1][starC1][starR2][starC2] = 0
    
    def move(r1, c1, r2, c2, direction, portalA1, portalA2, portalB1, portalB2):
        if direction == 0:
            R1, C1, R2, C2 = r1-1, c1, r2+1, c2
        elif direction == 1:
            R1, C1, R2, C2 = r1, c1+1, r2, c2-1
        elif direction == 2:
            R1, C1, R2, C2 = r1+1, c1, r2-1, c2
        else:
            R1, C1, R2, C2 = r1, c1-1, r2, c2+1

        if R1 < 0:  R1 = 0
        elif R1 >= len_r:   R1 = len_r-1

        if R2 < 0:  R2 = 0
        elif R2 >= len_r:   R2 = len_r-1

        if C1 < 0:  C1 = 0
        elif C1 >= len_c:   C1 = len_c-1

        if C2 < 0:  C2 = 0
        elif C2 >= len_c:   C2 = len_c-1

        pos1, pos2 = mapA[R1][C1], mapB[R2][C2]

        if pos1 == 2:   R1, C1 = r1, c1         #Obstacle
        elif pos1 == 3: R1, C1 = starR1, starC1 #Trap
        elif pos1 == 4: R1, C1 = portalA2       #Portal 1
        elif pos1 == 5: R1, C1 = portalA1       #Portal 2

        if pos2 == 2:   R2, C2 = r2, c2
        elif pos2 == 3: R2, C2 = starR2, starC2
        elif pos2 == 4: R2, C2 = portalB2
        elif pos2 == 5: R2, C2 = portalB1

        return R1, C1, R2, C2
    
    dq = deque([('', starR1, starC1, starR2, starC2)])

    route_q = deque([1,2,2,1,1,1,1,0,0,0,0,3,0,0,3,3,3,3,3,2])

    len_route = 0
    while dq:
        route, r1, c1, r2, c2 = dq.popleft()
        now = map_memo[r1][c1][r2][c2]

        for d in range(4):
            R1, C1, R2, C2 = move(r1, c1, r2, c2, d, portalA1, portalA2, portalB1, portalB2)
            if mapA[R1][C1] == 1 and mapB[R2][C2] == 1:     #Candy
                return route + str(d)

            if map_memo[R1][C1][R2][C2] > now+1:
                map_memo[R1][C1][R2][C2] = now+1
                dq.append(  (route + str(d), R1, C1, R2, C2)    )

        if len(route) > len_route:
            len_route = len(route)
            print(len_route)
            if len_route > 50:
                return None
            
mapA = [[0,0,0,0,0,2,0,2,0,0,0],
        [0,0,0,2,3,0,0,0,0,0,0],
        [0,0,0,3,0,5,0,3,0,2,0],
        [0,0,2,0,0,0,0,0,0,0,0],
        [0,0,0,3,0,0,2,0,0,2,0],
        [0,0,0,2,3,2,0,0,0,3,0],
        [0,2,4,0,0,0,0,0,0,2,0],
        [0,0,0,0,0,1,0,0,0,0,0],
        [0,0,2,3,0,0,0,3,2,0,0],
        [0,0,0,0,2,0,2,0,0,0,0]]

mapB = [[0,0,0,2,2,2,0,0,0,0,0],
        [0,2,2,2,2,2,3,2,2,0,0],
        [0,2,2,2,0,0,0,2,2,0,0],
        [0,0,0,3,0,0,3,2,2,0,0],
        [0,0,2,2,4,0,0,0,2,2,0],
        [0,0,2,2,2,2,0,0,2,2,0],
        [0,5,0,2,2,2,0,0,2,2,0],
        [0,0,0,0,0,0,1,2,2,2,0],
        [0,0,0,3,0,0,0,2,2,0,0],
        [0,0,0,0,0,0,0,0,0,0,0]]
starR1 = 3
starC1 = 6
starR2 = 3
starC2 = 5
print(main(mapA,mapB,starR1,starC1,starR2,starC2))
