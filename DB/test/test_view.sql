

#select * from tblHandGameList left join tblHandGameDetail on tblHandGameDetail.gameID = tblHandGameList.gameID where tblHandGameList.gameState =0 ORDER  BY tblHandGameList.gameID DESC ,tblHandGameDetail.subGameID LIMIT 0, 10 ;

select * from tblindianholdemlist left join tblindianholdemdetail on tblindianholdemlist.gameID = tblindianholdemdetail.gameID where tblindianholdemlist.gameState=0 ORDER  BY tblindianholdemlist.gameID DESC ,tblindianholdemdetail.subGameID
