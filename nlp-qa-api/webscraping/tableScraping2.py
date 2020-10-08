import re
from collections import defaultdict
import string
import json

#-----------------------------------------------------------------------------------#
def removeSpecialCharacters(string):
    string = re.sub('(\?|@|/|#|$|\"|\'|%|\\|&|\*|\(|\)|\^|")', ' ', string)
    #string = re.sub('\.', ' ', string)
    string = re.sub(':', ' ', string)
    string = re.sub('\n', ' ', string)
    string = re.sub('`', ' ', string)
    string = ' '.join(string.split())
    string = (string.replace('[',' ')).replace(']',' ')
    return string
#-----------------------------------------------------------------------------------#

#-----------------------------------------------------------------------------------#

#-----------------------------------------------------------------------------------#

#-----------------------------------------------------------------------------------#
def rSC(string):
    string = re.sub('(\?|/|#|$|\"|\'|\\|&|\*|\(|\)|\^|")', ' ', string)
    #string = re.sub('\.', ' ', string)
    string = re.sub(':', ' ', string)
    string = re.sub('\n', ' ', string)
    string = re.sub('`', ' ', string)
    string = ' '.join(string.split())
    string = (string.replace('[',' ')).replace(']',' ')
    return string


#-----------------------------------------------------------------------------------#
def table_to_json(dom):
    # ------------------------------------------------------------------------#
    table_data = []
    value = []

    for row in dom.findAll('tr'):
        aux = row.findAll('td')
        key = (aux[0].get_text()).strip()
        value = removeSpecialCharacters((aux[1].get_text()).strip())
       
        
        table_data.append({ "key" : key, "value" : [value]})
    print(table_data)

    return table_data
#############################################Samyak's Code###############################################


#--------------------------------------------------------------------------------------------------------#
def rs(string):
    string = string.replace('\n                          \t', ' - ')
    string = string.replace('\n                        ', '')
    string = string.replace(' \n                          ', '')
    string = string.replace('\n','')
    return string
#--------------------------------------------------------------------------------------------------------#


def table_wh1_to_json(dom):
    keys = [i.text for i in dom.find_all('th')]
    x=len(keys)
    
    
    final_my_dict = []

    for vale in dom.find_all('tbody'):
        data1 = [k.text for k in vale.find_all('tr')]
    y=len(data1)
       

    data = [j.text for j in dom.find_all('td')]

    for dre in dom.find_all('tbody'):
        data2 = [h.text for h in dre.find_all('a')]
    
    m=(len(data2))
    
    
    Valx=[]
    
    for e in range(0,m):
        for link in dom.find_all('a', class_= None , href=True, limit=e):
            if link['href'].endswith(".pdf") or link['href'].endswith(".PDF"):
                Valx.append(link['href'])
    
    
    
    Valx[m-m+0]=("https://ujjivansfb.in/" + (Valx[0]))
    Valx[m-m+1]=("https://ujjivansfb.in/" + (Valx[1]))
    Valx[m-m+2]=("https://ujjivansfb.in/" + (Valx[2]))
    Valx[m-m+4]=("https://ujjivansfb.in/" + (Valx[4]))
    
    
    k1=[dict.fromkeys(keys,0) for k in range(y)]
    for p in range(0,x):  
        k1[y-y][keys[p]]=data[p]
        k1[y-y+2][keys[p]]=data[p+x+x-1]

    for p in range(0,1):
        k1[y-y+1][keys[p]] = data[p]
        if y>=4:
            k1[y-y+3][keys[p]] = data[p+x+x-1]

    for p in range(0,x-1):
        k1[y-y+1][keys[p+1]] = data[p+x]
        if y>=4:
            k1[y-y+3][keys[p+1]] = data[p+x+x+x-1]

    k1[y-y][keys[2]] = data[2] + ' - ' + Valx[0]
    k1[y-y+1][keys[2]] = data[x+1] + ' - ' + Valx[1]
    k1[y-y+2][keys[4]] = data[x+x+3] + ' - ' + Valx[2]
    k1[y-y+2][keys[5]] = data[x+x+4] + ' - ' + Valx[3]
    k1[y-y+3][keys[4]] = data[x+x+x+2] + ' - ' + Valx[4]
    k1[y-y+3][keys[5]] = data[x+x+x+3] + ' - ' + Valx[5]

    for listVal in k1:
        res = ' :: '.join(['%s => %s' % (rs(key), rs(value)) for (key, value) in listVal.items()])
        final_my_dict.append(res)
    return final_my_dict
#------------------------------------------End of Fuction 1----------------------------------------------#

def table_wh2_to_json(dom):
    keys = [i.text for i in dom.find_all('th')]
    x=len(keys)
    final_my_dict = []

    for vale in dom.find_all('tbody'):
        data1 = [k.text for k in vale.find_all('tr')]

    for dre in dom.find_all('tbody'):
        data2 = [h.text for h in dre.find_all('a')]
    m=(len(data2))

    Valx=[]
    for e in range(0,m):
        for link in dom.find_all('a', class_= None , href=True, limit=e):
            if link['href'].endswith(".pdf") or link['href'].endswith(".PDF"):
                Valx.append(link['href'])
        
    Valx[m-m+1]=("https://ujjivansfb.in/" + (Valx[1]))
    Valx[m-m+3]=("https://ujjivansfb.in/" + (Valx[3]))        

    y=(len(data1) - 1)    
    data = [j.text for j in dom.find_all('td')]
    k1=[dict.fromkeys(keys,0) for k in range(y)]
    
    for p in range(0,x):  
        k1[y-y][keys[p]]=data[p]
        k1[y-y+1][keys[p]]=data[p+x]

    for p in range(0,1):
        k1[y-y+2][keys[p]] = data[p+x]
    
    for p in range(0,x-1):
        k1[y-y+2][keys[p+1]] = data[p+x+x]

    k1[y-y][keys[1]] = data[1] + ' - ' + Valx[0]
    k1[y-y+1][keys[3]] = data[x+3] + ' - ' + Valx[1]
    k1[y-y+1][keys[4]] = data[x+4] + ' - ' + Valx[2]
    k1[y-y+2][keys[3]] = data[x+x+2] + ' - ' + Valx[3]
    
    for listVal in k1:
        res = ' :: '.join(['%s => %s' % (rs(key),rs(value)) for (key, value) in listVal.items()])
        print(res)
        final_my_dict.append(res)
    return final_my_dict
    #------------------------------------------End of Function 2---------------------------------------------#

def sd(x,y):
    pb={
        "text" : [y]
    }
    an={
        "text" : x,
        "content" : [pb
        ]  
    }
     
    return an


def xb_to_json(dom):
    xb = []
    xc = []
    xd = []
    xe = []
    
    for h3 in dom.find_all('h3'):
        xb.append(h3.text)
    #return xb

    for li in dom.find_all('li'):
        xc.append(rSC(li.text))
    #return xc

    for p in dom.find_all('p'):
        xd.append(rSC(p.text))
    #print(xd)

    for strong in dom.find_all('strong'):
        xe.append(rSC(strong.text))
    print(xe)
