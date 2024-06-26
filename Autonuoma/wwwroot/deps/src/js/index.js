//pull in jQuery and plugins
import $ from 'jquery'

import 'cldrjs'
import Globalize from 'globalize'

import 'jquery-validation'
import 'jquery-validation-globalize'
import 'jquery-validation-unobtrusive'

import 'jquery-datetimepicker'

import 'datatables.net'

//pull in bootstrap scripts
import 'bootstrap'

//make styles known to webpack
import '../css/index.scss'

//expose jQuery outside the module built by webpack
window.__jQuery = $;
