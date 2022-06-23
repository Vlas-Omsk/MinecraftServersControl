let socket;
let counter = 0;

const serverEndPoint = 'ws://127.0.0.1:8888/gateway'

const RequestCode = Object.freeze({
	Auth: 1,
	GetServers: 2,
	StartServer: 3,
	TerminateServer: 4,
	GetOutput: 5,
	Input: 6
})

const ResponseCode = Object.freeze({
	Success: 0,
	DataError: 1,
	InternalServerError: 2,
	InvalidCode: 3,
	InvalidState: 4,
	SessionExpired: 5
})

async function GetHttpResponse(method, path, data, headers = {}) {
	const options = {
		method: method,
		mode: 'cors',
		cache: 'no-cache',
		credentials: 'same-origin',
		headers,
		redirect: 'follow',
		referrerPolicy: 'no-referrer'
	}
	
	if (data) {
		options.headers['Content-Type'] = 'application/json'
		options.body = JSON.stringify(data)
	}
	
	return await (await fetch('http://127.0.0.1:8888' + path, options)).json();
}

async function Connect(endPoint) {
	return new Promise((resolve, reject) => {
		const onOpen = function (event) {
			console.log('Connected to ' + endPoint)
			
			socket.removeEventListener('open', onOpen)
			resolve()
		}
		const onClose = function (event) {
			console.log('Disconnected from ' + endPoint)
			
			socket.removeEventListener('close', onClose)
			reject("Disconnected")
		}
		
		socket = new WebSocket(endPoint)
		socket.addEventListener('open', onOpen)
		socket.addEventListener('close', onClose)
	})
}

async function GetResponse(code, data) {
	let id = counter++;
	
	return new Promise((resolve, reject) => {
		const onMessage = function (event) {
			const json = JSON.parse(event.data)
			
			if (json.RequestId === id) {
				socket.removeEventListener('message', onMessage)
				
				if (json.Code === ResponseCode.Success)
					resolve(json)
				else
					reject(ResponseToLogArray(json))
			}
		}
		
		socket.addEventListener('message', onMessage)
		socket.send(JSON.stringify({
			Id: id,
			Code: code,
			Data: data
		}))
	})
}

async function Start() {
	let result = await GetHttpResponse('post', '/user/signin',
		{
			Login: "Admin",
			PasswordHash: "c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f"
		})
		
	let sessionId = result.Data.SessionId
	
	console.log("SignIn success: " + sessionId)
	
	result = await GetHttpResponse('get', '/user/restore', null,
		{
			Authorization: sessionId
		})
		
	sessionId = result.Data.SessionId
	
	console.log("Restore success: " + sessionId)
	
	await Connect(serverEndPoint)
	
	let response = await GetResponse(
		RequestCode.Auth,
		//"00" + sessionId.substr(2)
		sessionId
	)
	
	console.log(...ResponseToLogArray(response))

	response = await GetResponse(
		RequestCode.GetServers,
		null
	)
	
	console.log(...ResponseToLogArray(response))
	
	// Waiting for next messages
}

function ResponseToLogArray(response) {
	return [
		GetPropertyName(ResponseCode, response.Code),
		response.ErrorMessage
	]
}

function GetPropertyName(obj, value) {
	for (let key of Object.keys(obj))
		if (obj[key] === value)
			return key;
}

Start()