
#call sp_RunIndianHoldem(2, "14", "내가이긴다", "18-bc065091-bbe8-11e6-80c5-5a0000411ee7", 2, 2)

#call sp_EdtIndianHoldem(17, 2, "31-6d845f1b-c6a7-11e6-ada2-30a8b7a0fb08");

#call sp_loginSocial(1,"1","마니라네","1.png","psmon@live.co.kr");


/*

﻿#Select UNIX_TIMESTAMP(current_timestamp)
#Register Test
#call sp_deleteuser(0, "psmon99@live.co.kr");
#call sp_loginuser(0, "psmon@live.co.kr", "p102938");
#call sp_validtoken("26-4116234b-b4e0-11e6-b256-5a0000411ee7");
#select * from tblmember;
#select * from tblloginstate;


#GameTest
#call sp_creategame(0, 0, 0, 0, "", "사나이는빠", "gt-1234", 3, 200, '2', 1000, "18-fa0c5f04-b343-11e6-9da7-001c4274a6b1");
#call sp_rungame(16, '1', '18-fa0c5f04-b343-11e6-9da7-001c4274a6b1', '나는빠', 2);
#select Count(*)  from tblHandGameList where tblHandGameList.memberID_left = 17 and gameState =0;
#SqlTest
#select * from tblHandGameList where gameID=11;
*/

#Point Test
#select sum(earnPoint) FROM tblhandgamedetail inner join tblhandgamelist on tblhandgamedetail.gameID = tblhandgamelist.gameID  where winnerID=17;
#select sum(earnPoint),winnerID,nick FROM tblhandgamedetail inner join tblmember on tblhandgamedetail.winnerID = tblmember.memberID  group by winnerID;


#call sp_earnpoint("",17,"2017-01-01");

#call sp_earnpointList("2017-1-01",1);

#call sp_RunPublicIndianHoldem(36,"5","도전합니다.",1,2,"2Card high 점수(2)","5Card high 점수(5)","현정67","/img/avatar/default/avartar_34.png");

#call sp_CreateIndianHoldem(1,"무적행운","21:10:5",1,10,

#1480900768

call sp_RunQHoldem(33, 0, "ttt", "17-55ea192d-d9d2-11e6-ae85-32afd9316a7f", "1:2:3:4:5", 1, "1:2:3:4:5-6:7", "1:2:3:4:5-8:9");


