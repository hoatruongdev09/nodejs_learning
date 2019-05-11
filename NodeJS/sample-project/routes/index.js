// Full Documentation - https://www.turbo360.co/docs
const turbo = require('turbo360')({ site_id: process.env.TURBO_APP_ID })
const vertex = require('vertex360')({ site_id: process.env.TURBO_APP_ID })
const router = vertex.router()

const profiles = {
	sjobs: {
		image: '/images/stevejobs.jpg',
		name: 'steve jobs',
		company: 'apple',
		languages: ['objective-c', 'swift', 'c++']
	},
	bgates: {
		image: '/images/bgate.jpeg',
		name: 'bill gates',
		company: 'microsoft',
		languages: ['c', 'c#', 'java']
	},

}

router.get('/', (req, res) => {
	res.render('index', { text: 'This is the dynamic data. Open index.js from the routes directory to see.' })
})

router.post('/addprofile', (req, res) => {
	const body = req.body
	body['languages'] = req.body.languages.split(', ')
	console.log(JSON.stringify(body))
	profiles[body.username] = body
	res.redirect('/profile/' + body.username)

})

router.get('/query', (req, res) => {
	const name = req.query.name
	const occupation = req.query.occupation;
	const data = {
		name: name,
		occupation: occupation
	}
	res.render('profile', data);
	// res.json({
	// 	name: name,
	// 	occupation: occupation
	// })
})

router.get('/:path', (req, res) => {
	const path = req.params.path
	res.json({
		data: path,
	})
})
router.get('/:profile/:username', (req, res) => {
	const profile = req.params.profile
	const username = req.params.username
	const currentProfile = profiles[username]
	if (currentProfile == null) {
		res.json({
			confirmation: 'fail',
			message: 'profile ' + username + ' not found'
		})
	}
	res.render('profile', currentProfile)
})

module.exports = router
