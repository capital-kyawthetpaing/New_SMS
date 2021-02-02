/****** Object:  StoredProcedure [dbo].[D_Shipping_SelectForYamato]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Shipping_SelectForYamato]
GO

/****** Object:  StoredProcedure [D_Shipping_SelectForYamato]    */
CREATE PROCEDURE D_Shipping_SelectForYamato(
    -- Add the parameters for the stored procedure here
    @ShippingNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DS.LinkageDateTime
          ,(SELECT top 1 M.StoreCD FROM M_Souko AS M 
            WHERE M.SoukoCD = DS.SoukoCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    StoreCD
          
          --CSV���ڏ�
          ,DS.ShippingNO                         AS    col1    --�o�׎w���ԍ�	
          ,1                                     AS    col2    --�o�׎w�����הԍ�  
          ,(SELECT top 1 M.SCatKBN1 
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col3    --�������    
          ,(SELECT top 1 ISNULL(M.SCatKBN1 ,DS.BoxSize)
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col4    --�T�C�Y�i�ڃR�[�h  
          ,0                                     AS    col5    --�N�[���敪    
          ,Null                                  AS    col6    --�M�t�g�t���O  
          ,Null                                  AS    col7    --�󒍋敪  
          ,Null                                  AS    col8    --�����ԍ�    
          ,CONVERT(varchar,DS.ShippingDate,111)  AS    col9    --�o�ד�    
          ,CONVERT(varchar,DS.DecidedDeliveryDate,111) 
                                                 AS    col10   --���͂��\���  
          ,DS.DecidedDeliveryTime                AS    col11   --�[�i���Ԏw��敪  
          ,Null                                  AS    col12   --�����҃R�[�h  
          ,Null                                  AS    col13   --�����ғd�b�ԍ�    
          ,Null                                  AS    col14   --�����җX�֔ԍ�    
          ,Null                                  AS    col15   --�����ҏZ��    
          ,Null                                  AS    col16   --�����ҏZ���P  
          ,Null                                  AS    col17   --�����ҏZ���Q  
          ,Null                                  AS    col18   --�����ҏZ���R  
          ,Null                                  AS    col19   --�����Ҏ���    
          ,Null                                  AS    col20   --�����Ҍh��    
          ,Null                                  AS    col21   --�X�܃R�[�h    
          ,Null                                  AS    col22   --�X�ܓd�b�ԍ�  
          ,Null                                  AS    col23   --�X��FAX�ԍ�   
          ,Null                                  AS    col24   --�X�ܗX�֔ԍ�  
          ,Null                                  AS    col25   --�X�܏Z��  
          ,Null                                  AS    col26   --�X�܏Z���P    
          ,Null                                  AS    col27   --�X�܏Z���Q    
          ,Null                                  AS    col28   --�X�܏Z���R    
          ,Null                                  AS    col29   --�X�ܖ�    
          ,Null                                  AS    col30   --�X��URL   
          ,Null                                  AS    col31   --�X�܃��[���A�h���X    
          ,Null                                  AS    col32   --������    
          ,Null                                  AS    col33   --���ϕ��@�\��  
          ,Null                                  AS    col34   --���v  
          ,Null                                  AS    col35   --����  
          ,Null                                  AS    col36   --���ϗ�    
          ,Null                                  AS    col37   --�萔���P  
          ,Null                                  AS    col38   --�萔���Q  
          ,Null                                  AS    col39   --�萔���R  
          ,Null                                  AS    col40   --�������z  
          ,Null                                  AS    col41   --�������z  
          ,Null                                  AS    col42   --���͂���R�[�h    
          ,DI.DeliveryTelphoneNO                 AS    col43   --���͂���d�b�ԍ�  
          ,DI.DeliveryZip1CD + '-' + DI.DeliveryZip2CD
                                                 AS    col44   --���͂���X�֔ԍ�  
          ,LTRIM(RTRIM(DI.DeliveryAddress1)) + '�@' + LTRIM(RTRIM(DI.DeliveryAddress2))
                                                 AS    col45   --���͂���Z��  
          ,Null                                  AS    col46   --���͂���Z���P    
          ,Null                                  AS    col47   --���͂���Z���Q    
          ,Null                                  AS    col48   --���͂���Z���R    
          ,Null                                  AS    col49   --���͂��敔�喼�P  
          ,Null                                  AS    col50   --���͂��敔�喼�Q  
          ,DI.DeliveryName                       AS    col51   --���͂��於    
          ,Null                                  AS    col52   --���͂��於���̃J�i    
          ,Null                                  AS    col53   --���͂���h��  
          ,Null                                  AS    col54   --�˗���R�[�h  
          ,(SELECT top 1 W.TelephoneNO 
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col55   --�˗���d�b�ԍ�    
          ,(SELECT top 1 W.ZipCD1 + '-' + W.ZipCD2
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col56   --�˗���X�֔ԍ�    
          ,(SELECT top 1 LTRIM(RTRIM(W.Address1)) + '�@' + LTRIM(RTRIM(W.Address2))
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col57      --�˗���Z��    
          ,Null                                  AS    col58      --�˗���Z���P   
          ,Null                                  AS    col59      --�˗���Z���Q   
          ,Null                                  AS    col60      --�˗���Z���R   
          ,(SELECT top 1 W.InvoiceNotation 
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col61      --�˗��喼   
          ,Null                                  AS    col62      --�˗��喼���̃J�i   
          ,(SELECT M.Char1
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col63      --�׈����P �u�������m���Ӂv��\��  
          ,(SELECT M.Char2
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col64      --�׈����Q   
          ,Null                                  AS    col65      --�L��   
          ,Null                                  AS    col66      --�i���R�[�h�P   
          ,(SELECT M.Char3
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col67      --�i���P �u�ʔ̏��i�i�X�|�[�c�p�i�E�H�i�j�v��\��
          ,Null                                  AS    col68      --�i���R�[�h�Q   
          ,(SELECT M.Char4
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col69      --�i���Q 
          ,DI.CashOnAmount                       AS    col70      --�R���N�g��������z 
          ,DI.CashOnIncludeTax                   AS    col71      --�R���N�g������Ŋz��   
          ,0                                     AS    col72      --�c�Ə��~�u��   
          ,0                                     AS    col73      --�c�Ə��R�[�h   
          ,DS.UnitsCount                         AS    col74      --���� 
          ,0                                     AS    col75      --�����g�̈� 
          ,Null                                  AS    col76      --�̂������� 
          ,Null                                  AS    col77      --�o�׎w�����l�P 
          ,Null                                  AS    col78      --�o�׎w�����l�Q 
          ,Null                                  AS    col79      --�o�׎w�����l�R 
          ,Null                                  AS    col80      --�o�׎w�����l�S 
          ,Null                                  AS    col81      --�^��������R�[�h   
          ,Null                                  AS    col82      --�^��������R�[�h�}��   
          ,Null                                  AS    col83      --�^���Ǘ��ԍ�   
          ,0                                     AS    col84      --�o�׋敪   
          ,Null                                  AS    col85      --���i�ԍ�   
          ,Null                                  AS    col86      --���i���\�� 
          ,Null                                  AS    col87      --�o���G�[�V���� 
          ,Null                                  AS    col88      --���i��   
          ,Null                                  AS    col89      --�P��   
          ,Null                                  AS    col90      --���i�ō��P��   
          ,Null                                  AS    col91      --���i���z   
          ,Null                                  AS    col92      --���ח\�����ڂP 
          ,Null                                  AS    col93      --���ח\�����ڂQ 
          ,0                                     AS    col94      --���͂��\��e���[���@���p�敪    
          ,Null                                  AS    col95      --���͂��\��e���[���@e-mail�A�h���X  
          ,Null                                  AS    col96      --���͋@��   
          ,Null                                  AS    col97      --���͂��\��e���[���@���b�Z�[�W  
          ,0                                     AS    col98      --���͂�����e���[���@���p�敪    
          ,Null                                  AS    col99      --���͂�����e���[���@e-mail�A�h���X  
          ,Null                                  AS    col100     --���͂�����e���[���@���b�Z�[�W  
          ,Null                                  AS    col101     --�o�׋��_�R�[�h 
          ,0                                     AS    col102     --���񂵂񌈍ρ@���p�敪 
          ,Null                                  AS    col103     --���񂵂񌈍ρ@��t�ԍ� 
          ,Null                                  AS    col104     --���񂵂񌈍ρ@�����X�R�[�h 
          ,Null                                  AS    col105     --���񂵂񌈍ρ@������   
          ,Null                                  AS    col106     --���񂵂񌈍ρ@����於 
          ,Null                                  AS    col107     --���񂵂񌈍ρ@���ϋ��z�i�ō��j 
          ,Null                                  AS    col108     --���񂵂񌈍ρ@���ID   
          ,Null                                  AS    col109     --�\�����ڂP 
          ,Null                                  AS    col110     --�\�����ڂQ 
          ,0                                     AS    col111     --�����\�胁�[�����p�敪 
          ,Null                                  AS    col112     --�����\�胁�[���A�h���X 
          ,Null                                  AS    col113     --�����\�胁�[�����b�Z�[�W   
          ,0                                     AS    col114     --�����������[��(���͂��戶)���p�敪 
          ,Null                                  AS    col115     --�����������[��(���͂��戶)�A�h���X 
          ,Null                                  AS    col116     --�����������[��(���͂��戶)���b�Z�[�W   
          ,0                                     AS    col117     --�����������[��(���˗��制)���p�敪 
          ,Null                                  AS    col118     --�����������[��(���˗��制)�A�h���X 
          ,Null                                  AS    col119     --�����������[��(���˗��制)���b�Z�[�W   
          ,(SELECT top 1 M.SCatKBN2 
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col120     --�^����Ћ敪   
          ,0                                     AS    col121     --���Z���[�敪   
          ,Null                                  AS    col122     --�d��   
          ,0                                     AS    col123     --�ی����z   
          ,Null                                  AS    col124     --�f�[�^���ʋ敪 
          ,Null                                  AS    col125     --���X���S���� 
          ,Null                                  AS    col126     --���I��A�g�Ǘ��ԍ�   
          ,Null                                  AS    col127     --���I�𒼉c�X�R�[�h   
          ,Null                                  AS    col128     --���I���׎�l�R�[�h   
          ,Null                                  AS    col129     --�^�׎�R�[�h   
          ,Null                                  AS    col130     --JP�Z�L�����e�B�t���O   

    FROM D_Shipping AS DS
    LEFT OUTER JOIN D_ShippingDetails AS DM
    ON DM.ShippingNO = DS.ShippingNO
    AND DM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Instruction AS DI
    ON DI.InstructionNO = DS.InstructionNO
    AND DI.DeleteDateTime IS NULL
    
    WHERE DS.DeleteDateTime IS NULL
    AND DS.ShippingNO = @ShippingNO
    ORDER BY DM.ShippingRows
    ;

END

GO
