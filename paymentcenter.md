# GW支付中心 技术手册

## 规范说明:

编码：`UTF-8`
 
Method：請求/回調 `POST` 查詢 `GET`

Content-Type： `application/json`

交易回调地址： 至 `支付中心` 设置

---

## [索引](#索引)
[1.接口地址](#1接口地址)

[2.验签方式](#2验签方式)

[3.存款](#3存款)

[4.提款](#4提款)

[5.异步回调](#5异步回调)

[6.支付方式](#6支付方式)

[7.支付类型](#7支付类型)

[8.状态码](#8状态码)

---

### 1.接口地址

| 名称 | 地址 | Http-Method |
|:------:|------|:------:|
|存款请求|http://203.60.2.122:61830/deposits    | `POST` |
|存款查询|http://203.60.2.122:61830/deposits    | `GET` |
|提款请求|http://203.60.2.122:61830/withdrawals | `POST` |
|提款查询|http://203.60.2.122:61830/withdrawals | `GET` |

---

### 2.验签方式

#### 说明：

1. 参数列表中，除去 sign 参数外，其他所有非空参数都要参与签名。

2. 参与签名的参数顺序按照首字母从小到大（a 到 z）的顺序排列， 如果遇到相同首字母则按第二个字母从小到大的顺序排列，以此类推。 

3. 签名以 `AES` 加密

4. 参照 demo 执行加密流程 `可向支付中心技术人员索取 demo 代码`

#### 举例：

准备好以下：

1. 商户密钥(merchantKey) `由支付中心配发`

2. 待签原文： `参数名1=参数值1&参数名2=参数值2&......参数名n=参数值n`
   > 待签原文： `amount=50.00&bankCode=example.....propertynameN=valueN`
   
3. 开始AES加密
   > `AES.Key=SHA256.ComputeHash(merchantKey)`   
   > `AES.IV=MD5.ComputeHash(merchantKey)`
   
4. Sign Result: `eI3fAdLSsPRqEuQ4CG6YMyYjlASHPAO1DTpz6srsysq7xiBYr7yalu2brpYMWac4bK/cVtK1R+yLrY33+2LkIQ==`
---

### 3.存款 

#### `重点内容`

[存款请求](#存款请求)

[存款请求响应](#存款请求响应)

[存款查询](#存款查询)

[存款查询响应](#存款查询响应)

---

#### 存款请求

##### 对象：
> 商户→支付中心

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `merchantCode` | 商户号 | :heavy_check_mark: | 由支付中心提供的唯一标识号 | ap20180820001 |
| `merchantOrderNo` | 商户订单号 | :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `payMethod` | 支付类型| :heavy_check_mark: | 請參考 [7.支付类型](#7支付类型) | |
| `plat` | 终端设备| :heavy_check_mark: | 請參考 [9.终端设备](#9终端设备) | |
| `reqAmount` | 订单金额 | :heavy_check_mark: | 單位:元 保留至小数点第2位 | 100.00 |
| `username` | 使用者姓名 | :heavy_check_mark: | 部分支付商需要此参数 | 技术人员 |
| `ip` | 使用者ip | :heavy_check_mark: | 部分支付商需要此参数 | 192.168.1.1(需为真实ip) |
| `bankCode` |银行代码 | | 支付类型选择 `网银`、`在线支付` 需提供 | 至 `支付中心后台` 设置对应支付商银行代码 |
| `returnUrl` |跳转地址| :heavy_check_mark: | 使用者付款成功后，跳转的网页(通常为第三方付款画面)| https://www.example.com |
| `payerName` | 付款人姓名 | | 支付类型选择 `支付宝转卡` 需提供 | 技术人员 |
| `cardSN` | 充值卡号 | | 支付类型选择充值卡需提供 | SN123456789 |
| `cardPwd` | 充值卡密码 | | 支付类型选择充值卡需提供 | 123456 |
| `sign` | 签名 | :heavy_check_mark: | 签名数据 | 请参考 [2.验签方式](#2验签方式) |

>> [存款](#3存款) | [索引](#索引)

---

#### 存款请求响应

##### 对象：
> 支付中心→商户

reqStatus若失败，仅提供 `reqStatus` `noticeType` `errorMsg`，且不验签

|参数|参数名称&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `reqStatus` | 请求状态 | :heavy_check_mark: | 请求是否成功 | 请参考 [8.状态码](#8状态码) |
| `merchantOrderNo` | 商户订单号 | :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `systemOrderNo` | 支付中心订单号 | :heavy_check_mark: | 支付中心提供给商户的单号 | CT180820120505001 |
| `paymentOrderNo` | 支付订单号 | | 支付商提供给支付中心的单号，转传给商户 | ALI1808201205105001 |
| `status ` | 状态 | :heavy_check_mark: | 处理状态 | 请参考 [8.状态码](#8状态码) |
| `noticeType ` | 来源代码 |  | `1`:支付中心反馈信息 `2`:支付商反馈信息 | |
| `errorMsg ` | 错误訊息 |  | 若 `reqStatus` 或 `status` 失败时必填，并参考 `noticeType` | 签名错误 |
| `payContent` | 支付内容 | | 通常情況返回網址，网银转帐信息則是json | https://mobile.alipay.com/index.htm?cid=example |
| `sign` | 签名 | | 签名数据，`reqStatus` 失败时，不验签 | 请参考 [2.验签方式](#2验签方式) |

>> [存款](#3存款) | [索引](#索引)

---

#### 存款查询

##### 对象：
> 商户→支付中心

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `merchantCode` | 商户号 | :heavy_check_mark: | 由支付中心提供的唯一标识号 | ap20180820001 |
| `merchantOrderNo` |商户订单号| :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `payMethod` | 支付类型| :heavy_check_mark: | 請參考 [7.支付类型](#7支付类型) | |
| `sign` | 签名 | :heavy_check_mark: | 签名数据 | 请参考 [2.验签方式](#2验签方式) |

>> [存款](#3存款) | [索引](#索引)

---

#### 存款查询响应

##### 对象：
> 支付中心→商户

reqStatus若失败，仅提供 `reqStatus` `noticeType` `errorMsg`，且不验签

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `reqStatus` | 请求状态 | :heavy_check_mark: | 请求是否成功 | 请参考 [8.状态码](#8状态码) |
| `reqAmount` | 订单金额 | :heavy_check_mark: | 單位:元 保留至小数点第2位 | 100.00 |
| `amount` | 实际金额 | :heavy_check_mark: | | 單位:元 保留至小数点第2位 | 100.00 |
| `merchantOrderNo` | 商户订单号 | :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `systemOrderNo` | 支付中心订单号 | :heavy_check_mark: | 支付中心提供给商户的单号 | CT180820120505001 |
| `paymentOrderNo` | 支付订单号 |  | 支付商提供给支付中心的单号，转传给商户 | ALI1808201205105001 |
| `status ` | 状态 | :heavy_check_mark: | 处理状态 | 请参考 [8.状态码](#8状态码) |
| `noticeType ` | 来源代码 | | `1`： 支付中心反馈信息 `2`： 支付商反馈信息 | |
| `errorMsg ` | 错误訊息 |  | 若 `reqStatus` 或 `status` 失败时必填，并参考 `noticeType` | 签名错误 |
| `sign` | 签名 | | 签名数据，`reqStatus` 失败时，不验签 | 请参考 [2.验签方式](#2验签方式) |

>> [存款](#3存款) | [索引](#索引)

---

### 4.提款 

#### `重点内容`

[提款请求](#提款请求)

[提款请求响应](#提款请求响应)

[提款查询](#提款查询)

[提款查询响应](#提款查询响应)

---

#### 提款请求

##### 对象：
> 商户→支付中心

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `merchantCode` | 商户号 | :heavy_check_mark: | 由支付中心提供的唯一标识号 | ap20180820001 |
| `merchantOrderNo` |商户订单号| :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `reqAmount` | 订单金额| :heavy_check_mark: | 單位:元 保留至小数点第2位 | 100.00 |
| `username` | 使用者姓名 | :heavy_check_mark: | 部分支付商需要此参数 | 技术人员 |
| `plat` | 终端设备| :heavy_check_mark: | 請參考 [9.终端设备](#9终端设备) | |
| `ip` | 使用者ip | :heavy_check_mark: | 部分支付商需要此参数 | 192.168.1.1(需为真实ip) |
| `bankCode` |银行代码 | :heavy_check_mark: | 客户提款银行代码，至 `支付中心后台` 设置对应支付商银行编码 | CCB |
| `payeeCardNumber` | 银行卡号 | :heavy_check_mark: | 客户的提款卡号 | 1231236046598712 |
| `payeeName` | 银行卡姓名 | :heavy_check_mark: | 客户的提款卡姓名 | 技术员 |
| `sign` | 签名 | :heavy_check_mark: | 签名数据 | 请参考 [2.验签方式](#2验签方式) |

>> [提款](#4提款) | [索引](#索引)

---

#### 提款请求响应

##### 对象：
> 支付中心→商户

reqStatus若失败，仅提供 `reqStatus` `noticeType` `errorMsg`，且不验签

|参数|参数名称&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `reqStatus` | 请求状态 | :heavy_check_mark: | 请求是否成功 | 请参考 [8.状态码](#8状态码) |
| `merchantOrderNo` | 商户订单号 | :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `systemOrderNo` | 支付中心订单号 | :heavy_check_mark: | 支付中心提供给商户的单号 | CT180820120505001 |
| `paymentOrderNo` | 支付订单号 |  | 支付商提供给支付中心的单号，转传给商户 | ALI1808201205105001 |
| `status ` | 状态 | :heavy_check_mark: | 处理状态 | 请参考 [8.状态码](#8状态码) |
| `noticeType ` | 来源代码 |  | `1`：支付中心反馈信息 <br/> `2`：支付商反馈信息 | |
| `errorMsg ` | 错误訊息 |   | 若 `reqStatus` 或 `status` 失败时必填，并参考 `noticeType` | 签名错误 |
| `payContent` | 支付内容 | | `必定为空，提款请忽略此栏位` |
| `sign` | 签名 | | 签名数据，`reqStatus` 失败时，不验签 | 请参考 [2.验签方式](#2验签方式) |

>> [提款](#4提款) | [索引](#索引)

---

#### 提款查询

##### 对象：
> 商户→支付中心

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `merchantCode` | 商户号 | :heavy_check_mark: | 由支付中心提供的唯一标识号 | ap20180820001 |
| `merchantOrderNo` |商户订单号| :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | TK1808201205049001 |
| `sign` | 签名 | :heavy_check_mark: | 签名数据 | 请参考 [2.验签方式](#2验签方式) |

>> [提款](#4提款) | [索引](#索引)

---

#### 提款查询响应

##### 对象：
> 支付中心→商户

reqStatus若失败，仅提供 `reqStatus` `noticeType` `errorMsg`，且不验签

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `reqStatus` | 请求状态 | :heavy_check_mark: | 请求是否成功 | 请参考 [8.状态码](#8状态码) |
| `reqAmount` | 订单金额 | :heavy_check_mark: | 單位:元 保留至小数点第2位 | 100.00 |
| `amount` | 实际金额 | :heavy_check_mark: | | 單位:元 保留至小数点第2位 | 100.00 |
| `merchantOrderNo` | 商户订单号 | :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `systemOrderNo` | 支付中心订单号 | :heavy_check_mark: | 支付中心提供给商户的单号 | CT180820120505001 |
| `paymentOrderNo` | 支付订单号 | | 支付商提供给支付中心的单号，转传给商户 | ALI1808201205105001 |
| `status ` | 状态 | :heavy_check_mark: | 处理状态 | 请参考 [8.状态码](#8状态码) |
| `noticeType ` | 来源代码 | | `1`:支付中心反馈信息 `2`:支付商反馈信息 | |
| `errorMsg ` | 错误訊息 | | 若 `reqStatus` 或 `status` 失败时必填，并参考 `noticeType` | 签名错误 |
| `sign` | 签名 | | 签名数据，`reqStatus` 失败时，不验签 | 请参考 [2.验签方式](#2验签方式) |

>> [提款](#4提款) | [索引](#索引)

---

### 5.异步回调

#### `重点内容`

[异步回调](#异步回调)

[异步回调响应](#异步回调响应)

---

#### 异步回调

##### 对象：
> 支付中心→商户

##### 规则：
若交易成功，当下主动传送一次通知，之后每5分钟传送一次，直到收到响应 `SUCCESS`

|参数|参数名称|必填|说明|举例|
|:---:|:--------:|:---:|----|----|
| `merchantCode` | 商户号 | :heavy_check_mark: | 由支付中心提供的唯一标识号 | ap20180820001 |
| `merchantOrderNo` |商户订单号| :heavy_check_mark: | 商户提供订单号给支付中心，需为唯一 | CK1808201205049001 |
| `systemOrderNo` | 支付中心订单号 | :heavy_check_mark: | 支付中心提供给商户的单号 | CT180820120505001 |
| `paymentOrderNo` | 支付订单号 | :heavy_check_mark: | 支付商提供给支付中心的单号，转传给商户 | ALI1808201205105001 |
| `payType` | 支付方式| :heavy_check_mark: | 請參考 [6.支付方式](#6支付方式) | |
| `payMethod` | 支付类型| :heavy_check_mark: | 請參考 [7.支付类型](#7支付类型) | |
| `reqAmount` | 订单金额 | :heavy_check_mark: | 單位:元 保留至小数点第2位 | 100.00 |
| `amount` | 实际金额 | :heavy_check_mark: | 單位:元 保留至小数点第2位 | 100.00 |
| `status ` | 状态 | :heavy_check_mark: | 处理状态 | 请参考 [8.状态码](#8状态码) |
| `sign` | 签名 | :heavy_check_mark: | 签名数据 | 请参考 [2.验签方式](#2验签方式) |

>> [异步回调](#5异步回调) | [索引](#索引)

---

#### 异步回调响应

##### 对象：
> 商户→支付中心

响应字串 `SUCCESS` 即可

>> [异步回调](#5异步回调) | [索引](#索引)

---

### 6.支付方式

| 名称 | 参数 |
|:----:|:---:|
| 存款 | 1 |
| 提款 | 2 |

>> [索引](#索引)

---

### 7.支付类型

| 名称 | 参数 |
|:----:|:---:|
| 微信 | 1 |
| 支付宝 | 2 |
| 在线支付 | 3 |
| 网银支付 | 4 |
| 充值卡 | 5 |
| QQ钱包 | 6 |
| 银联支付 | 7 |
| 支付宝转卡 | 8 |
| 京东钱包 | 9 |

>> [索引](#索引)

----

### 8.状态码

| 名称 | 参数 |
|:----:|:---:|
| 未知 | 0 |
| 成功 | 1 |
| 失败 | 2 |
| 支付中 | 3 |

>> [索引](#索引)

---

### 9.终端设备

| 名称 | 参数 |
|:----:|:---:|
| PC | 1 |
| 手机 | 2 |

>> [索引](#索引)
