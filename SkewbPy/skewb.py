from copy import deepcopy
from collections import deque
import sys

def R(cpos):
    pos = deepcopy(cpos)
    pos[2],pos[3],pos[5] = pos[5],pos[2],pos[3]
    pos[7],pos[18],pos[15],pos[12],pos[17],pos[27],pos[25],pos[29],pos[20] = pos[12],pos[17],pos[27],pos[25],pos[29],pos[20],pos[7],pos[18],pos[15]
    pos[21], pos[28], pos[16] = pos[16], pos[21], pos[28]
    return pos

def Rp(cpos):
    pos = deepcopy(cpos)
    pos[5],pos[2],pos[3] = pos[2],pos[3],pos[5]
    pos[12],pos[17],pos[27],pos[25],pos[29],pos[20],pos[7],pos[18],pos[15] = pos[7],pos[18],pos[15],pos[12],pos[17],pos[27],pos[25],pos[29],pos[20]
    pos[16], pos[21], pos[28] = pos[21], pos[28], pos[16]
    return pos

def L(cpos):
    pos = deepcopy(cpos)
    pos[1], pos[5], pos[4] = pos[4], pos[1], pos[5]
    pos[10], pos[9], pos[23], pos[12], pos[27], pos[17], pos[25], pos[29], pos[20] = pos[25], pos[20], pos[29], pos[23], pos[10], pos[9], pos[27], pos[12], pos[17]
    pos[13], pos[26], pos[24] = pos[24], pos[13], pos[26]
    return pos

def Lp(cpos):
    pos = deepcopy(cpos)
    pos[4], pos[1], pos[5] = pos[1], pos[5], pos[4]
    pos[25], pos[20], pos[29], pos[23], pos[10], pos[9], pos[27], pos[12], pos[17] = pos[10], pos[9], pos[23], pos[12], pos[27], pos[17], pos[25], pos[29], pos[20]
    pos[24], pos[13], pos[26] = pos[13], pos[26], pos[24]
    return pos

def U(cpos):
    pos = deepcopy(cpos)
    pos[4], pos[0], pos[3] = pos[0], pos[3], pos[4]
    pos[7], pos[18], pos[15], pos[10], pos[9], pos[23], pos[25], pos[29],pos[20] = pos[20], pos[25], pos[29], pos[15], pos[18], pos[7], pos[9], pos[10], pos[23]
    pos[6], pos[19], pos[22] = pos[19], pos[22], pos[6]
    return pos

def Up(cpos):
    pos = deepcopy(cpos)
    pos[0], pos[3], pos[4] = pos[4], pos[0], pos[3]
    pos[20], pos[25], pos[29], pos[15], pos[18], pos[7], pos[9], pos[10], pos[23] = pos[7], pos[18], pos[15], pos[10], pos[9], pos[23], pos[25], pos[29],pos[20]
    pos[19], pos[22], pos[6] = pos[6], pos[19], pos[22]
    return pos

def B(cpos):
    pos = deepcopy(cpos)
    pos[5], pos[3], pos[4] = pos[4], pos[5], pos[3]
    pos[19], pos[22], pos[6], pos[16], pos[21], pos[28], pos[24], pos[13], pos[26] = pos[28], pos[21], pos[16], pos[13], pos[26], pos[24], pos[19], pos[6], pos[22] 
    pos[20], pos[25], pos[29] = pos[29], pos[20], pos[25]
    return pos

def Bp(cpos):
    pos = deepcopy(cpos)
    pos[4], pos[5], pos[3] = pos[5], pos[3], pos[4]
    pos[28], pos[21], pos[16], pos[13], pos[26], pos[24], pos[19], pos[6], pos[22] = pos[19], pos[22], pos[6], pos[16], pos[21], pos[28], pos[24], pos[13], pos[26] 
    pos[29], pos[20], pos[25] = pos[20], pos[25], pos[29]
    return pos

mdic = {"R":R,"R'":Rp,"B":B,"B'":Bp,"L":L,"L'":Lp,"U":U,"U'":Up}

#scramble = "B R L' U' R U' R L'".split(' ')
#scramble = "R B' L B' L' U R' L'".split(' ')

solved = "W G R B O Y W W W W G G G G R R R R B B B B O O O O Y Y Y Y".split(' ')
start = "W G R B O Y W W W W G G G G R R R R B B B B O O O O Y Y Y Y".split(' ')

# for move in scramble:
#     start = mdic[move](start)
#     #print(start)

def solve(cpos):
    possmoves = ["U", "U'", "R", "R'", "L", "L'", "B","B'"]
    i = 0
    visited = set()
    scop = deepcopy(cpos)
    q = deque([(scop, [], 'k')])
    while q:
        s, path, last_move = q.popleft()
        if str(s) not in visited:
            visited.add(str((deepcopy(s))))
            for move in possmoves:
                if move[0] != last_move[0]:
                    pos = mdic[move](s)
                    #print(self.pos)
                    newpath = deepcopy(path)
                    newpath.append(move)
                    if pos == solved:
                        return newpath,i
                    q.append((pos,newpath,move))
                    #print(len(q))
                    i+=1
                    if i%10000 == 0:
                        print(i,len(newpath))

def dist():
    possmoves = ["U", "U'", "R", "R'", "L", "L'", "B","B'"]
    idx = 0
    # visited = set()
    scop = deepcopy(solved)
    overview = [set(),set(),set(),set(),set(),set(),set(),set(),set(),set(),set(),set()]
    overview[0].add(str(" ".join(scop)))
    # q = deque([(scop, [], 'k')])
    for i in range(12):
        for poss in overview[i]:
            for move in possmoves:
                    #print(poss)
                    pos = poss.split(' ')
                    #print(pos)
                    pos = mdic[move](pos)
                    strpos = str(" ".join(pos))
                    if strpos not in overview[(i-1)%12] and strpos not in overview[i]:
                        overview[i+1].add(strpos)
                    idx+=1
                    if idx%100000 == 0:
                        print(idx,i)
                        for kk, k in enumerate(overview):
                            print(kk,len(k))
    return overview

def layercheck(cpos):
    if cpos[0] == 'W':
        if cpos[6] == 'W' and cpos[7] == 'W' and cpos[8] == 'W' and cpos[9] == 'W' and cpos[10] == 'G':
            return True
    if cpos[1] == 'G':
        if cpos[10] == 'G' and cpos[11] == 'G' and cpos[12] == 'G' and cpos[13] == 'G' and cpos[9] == 'W':
            return True
    if cpos[2] == 'R':
        if cpos[14] == 'R' and cpos[15] == 'R' and cpos[16] == 'R' and cpos[17] == 'R' and cpos[7] == 'W':
            return True
    if cpos[3] == 'B':
        if cpos[18] == 'B' and cpos[19] == 'B' and cpos[20] == 'B' and cpos[21] == 'B' and cpos[7] == 'W':
            return True
    if cpos[4] == 'O':
        if cpos[22] == 'O' and cpos[23] == 'O' and cpos[24] == 'O' and cpos[25] == 'O' and cpos[6] == 'W':
            return True
    if cpos[5] == 'Y':
        if cpos[26] == 'Y' and cpos[27] == 'Y' and cpos[28] == 'Y' and cpos[29] == 'Y' and cpos[12] == 'G':
            return True
    else:
        return False

def laysolve(cpos):
    if layercheck(cpos):
        return []
    possmoves = ["U", "U'", "R", "R'", "L", "L'", "B","B'"]
    i = 0
    visited = set()
    scop = deepcopy(cpos)
    q = deque([(scop, [], 'k')])
    while q:
        s, path, last_move = q.popleft()
        if str(s) not in visited:
            visited.add(str((deepcopy(s))))
            for move in possmoves:
                if move[0] != last_move[0]:
                    pos = mdic[move](s)
                    #print(self.pos)
                    newpath = deepcopy(path)
                    newpath.append(move)
                    if layercheck(pos):
                        return newpath
                    q.append((pos,newpath,move))
                    #print(len(q))
                    i+=1
                    if i%10000 == 0:
                        print(i,len(newpath))

def laydist(ov):
    idx = 0
    overview = [set(),set(),set(),set(),set(),set(),set(),set(),set(),set(),set(),set()]
    for positions in ov:
        for state in positions:
            sstate = state.split(' ')
            overview[len(laysolve(sstate))].add(state)
            idx +=1
            if idx%5000 == 0:
                print(idx)
                for kk, k in enumerate(overview):
                    print(kk,len(k))
    return overview



#sol = laysolve(start)

ov = dist()
# ov2 = laydist(ov)
# for kk, k in enumerate(ov2):
#     print(kk,len(k))
print("Done")

# print(sol)