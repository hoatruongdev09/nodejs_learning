const express = require('express')
const router = express.Router()

router.get('/', function (req, res, next) {
    res.send('hellow from Routers folder !')
})

router.get('/home', function (req, res, next) {
    res.render('home', null);
})
router.get('/json', function (req, res, next) {
    res.json({
        greeting: 'hello from router folder'
    })
})
module.exports = router