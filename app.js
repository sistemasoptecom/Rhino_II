(function () {
    'use strict';
    angular
        .module('Rhino', ['ngRoute', 'ngResource', 'ngCookies', 'ngAnimate', 'ui.bootstrap', 'chart.js'])
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/Modulo', {
                    templateUrl: 'app/js/modulos.html',
                    controller: 'Select_module'
                })
                .when('/Login', {
                    templateUrl: 'app/js/Login.html',
                    controller: 'LoginController'
                })
                .when('/Home', {
                    templateUrl: 'app/js/Home.html',
                    controller: 'HomeController',
                    authenticated: true
                })
                .when('/', {
                    redirectTo: '/Modulo'
                })
                .otherwise("/", {
                    redirectTo: '/Modulo'
                });
        }])
        .run(function ($rootScope, Auth) {
            $rootScope.$on('$routeChangeStart', function () {
                Auth.checkStatus();
            });
        })
        .factory('Service', ['$http', Service])
        .factory('Auth', ['$cookies', '$cookieStore', '$location', Auth])
        .factory('ssDate', ['$filter', ssDate])
        .directive('fileread', fileread)
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {
                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value, 10);
                    });
                }
            };
        })
        .directive('ssMax', function () {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, elem, attrs, ctrl) {
                    attrs.$set("ngTrim", "false");
                    var maxlength = parseInt(attrs.ssMax, 10);
                    ctrl.$parsers.push(function (value) {

                        if (value == null) { value = ""; }
                        if ((value + "").length > maxlength) {
                            value = (value + "").substr(0, maxlength);
                            ctrl.$setViewValue(value);
                            ctrl.$render();
                        }
                        var aux;
                        if (value instanceof Number) {
                            aux = parseInt(value);
                        } else {
                            aux = value;
                        }
                        return aux;
                    });
                }
            };
        })
        .directive('ssCheck', function () {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (est) {
                        return est ? 1 : 0;
                    });
                    console.log(attrs);
                    console.log(element);

                }
            }
        })
        .directive('stringToTime', function (ssDate) {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {
                        return ssDate.dateToString(value, 'HH:mm');
                        ngModel.$formatters.push(function (value) {
                            if (value != undefined) {
                                return new Date(1999, 1, 1, value.split(':')[0], value.split(':')[1]);
                            }
                        });
                    });

                }
            };
        })
        .directive('onReadFile', function ($parse) {
            return {
                restrict: 'A',
                scope: false,
                link: function (scope, element, attrs) {
                    var fn = $parse(attrs.onReadFile);
                    element.on('change', function (onChangeEvent) {
                        var reader = new FileReader();
                        reader.onload = function (onLoadEvent) {
                            scope.$apply(function () {
                                fn(scope, { $fileContent: onLoadEvent.target.result });
                            });
                        };
                        reader.readAsText((onChangeEvent.srcElement || onChangeEvent.target).files[0]);
                    });
                }
            };
        })


        function Auth($cookies, $cookieStore, $location) {
            return {
                Login: function (UserObj) {
                    $cookieStore.put('UserObj', UserObj);
                    $location.path('/');
                },
                LogOut: function (UserObj) {
                    $cookieStore.remove('UserObj');
                    sessionStorage.clear();
                    $location.path('/');
                },
                checkStatus: function () {
                    var rutasPrivadas = ['/Home', '/'];
                    if (this.in_array($location.path(), rutasPrivadas) && $cookieStore.get('UserObj') == undefined) {
                        $location.path("/");
                    }
                    if (this.in_array($location.path(), rutasPrivadas) && $cookieStore.get('UserObj') != undefined) {
                        $location.path("/Home");
                    }
                },
                in_array: function (needle, haystack) {
                    var key = '';
                    for (key in haystack) {
                        if (haystack[key] == needle) {
                            return true;
                        }
                    }
                    return false;
                },
                getUserObj: function () {
                    return $cookieStore.get('UserObj');
                }
            };
    } 
        function ssDate($filter) {
                  return {
                      /*Calcular la edad con fecha de nacimiento en milisegundos*/
                      getEdad: function (mili) {
                          if (mili != null) {
                              var edad;
                              var fechaActula = new Date().getTime();
                              var fechaSel = mili;
                              var rs = fechaActula - fechaSel;
                              var rsr = ((((((rs / 1000) / 60) / 60) / 24) / 30.6) / 12);
                              var strEdad = "";
                              if (rsr == 1) {
                                  edad += " Año"
                              } else {
                                  strEdad = " Años";
                              }
                              if (rsr < 1) {
                                  rsr = rsr * 12;
                                  if (rsr == 1) {
                                      strEdad = " Mes"
                                  } else {
                                      strEdad = " Meses";
                                  }
                                  if (rsr < 1) {
                                      rsr = rsr * 30;
                                      if (rsr < 1 && rsr > 0) {
                                          rsr = 1;
                                      }
                                      if (rsr < 0) {
                                          rsr = 0;
                                          strEdad = "";
                                      }
                                      if (rsr < 2 && rsr > 0) {
                                          strEdad = " Dia"
                                      } else if (rsr > 2) {
                                          strEdad = " Dias";
                                      }
                                  }
                              }
                              return Math.floor(rsr) + " " + strEdad;
                          } else {
                              return "";
                          }

                      },
                      /*Calcular la edad con fecha de nacimiento en milisegundos*/
                      getEdad2: function (mili) {
                          if (mili != null) {
                              var edad;
                              var fechaActula = new Date().getTime();
                              var fechaSel = mili;
                              var rs = fechaActula - fechaSel;
                              var rsr = ((((((rs / 1000) / 60) / 60) / 24) / 30.6) / 12);
                              var strEdad = "";
                              if (rsr < 1) {
                                  rsr = rsr * 12;
                                  if (rsr == 1) {
                                      strEdad = " Mes"
                                  } else {
                                      strEdad = " Meses";
                                  }
                                  if (rsr < 1) {
                                      rsr = rsr * 30;
                                      if (rsr < 1 && rsr > 0) {
                                          rsr = 1;
                                      }
                                      if (rsr < 0) {
                                          rsr = 0;
                                          strEdad = "";
                                      }
                                      if (rsr < 2 && rsr > 0) {
                                          strEdad = " Dia"
                                      } else if (rsr > 2) {
                                          strEdad = " Dias";
                                      }
                                  }
                              }
                              return Math.floor(rsr) + " " + strEdad;
                          } else {
                              return "";
                          }

                      },
                      /*Calcular la edad con fecha de nacimiento en milisegundos*/
                      getEdad3: function (mili,fecha_actual) {
                          if (mili != null) {
                              var edad;
                              var fechaActula = fecha_actual.getTime();
                              var fechaSel = mili;
                              var rs = fechaActula - fechaSel;
                              var rsr = ((((((rs / 1000) / 60) / 60) / 24) / 30.6) / 12);
                              var strEdad = "";
                              if (rsr == 1) {
                                  edad += " Año"
                              } else {
                                  strEdad = " Años";
                              }
                              if (rsr < 1) {
                                  rsr = rsr * 12;
                                  if (rsr == 1) {
                                      strEdad = " Mes"
                                  } else {
                                      strEdad = " Meses";
                                  }
                                  if (rsr < 1) {
                                      rsr = rsr * 30;
                                      if (rsr < 1 && rsr > 0) {
                                          rsr = 1;
                                      }
                                      if (rsr < 0) {
                                          rsr = 0;
                                          strEdad = "";
                                      }
                                      if (rsr < 2 && rsr > 0) {
                                          strEdad = " Dia"
                                      } else if (rsr > 2) {
                                          strEdad = " Dias";
                                      }
                                  }
                              }
                              return Math.floor(rsr) + " " + strEdad;
                          } else {
                              return "";
                          }

                      },

                      /*Calcular fecha con tiempo en milisegundo*/
                      militodate: function (mili) {
                          if (mili != null) {
                              var tam = mili.length - 2;
                              var fechaString = $filter('date')(mili.substring(6, tam), 'yyyy,MM,dd');
                              return new Date(fechaString);
                          }
                          else {
                              return "";
                          }
                      },
                      /*Calcular la fecha y dale formato con tiempo en milisegundo*/
                      militodateString: function (mili) {
                          if (mili != null) {
                              var tam = mili.length - 2;
                              var fechaString = $filter('date')(mili.substring(6, tam), 'dd/MM/yyyy');
                              return fechaString;
                          } else {
                              return "";
                          }
                      },
                      /**/
                      militoHours: function (mili) {
                          if (mili != null) {
                              var hora = $filter('date')(mili, 'HH:mm');
                              return hora;
                          }
                          else {
                              return "";
                          }

                      },
                      /*Convertir string en hora(date)*/
                      stringToHours: function (_str) {
                          if (_str != null) {
                              var str = _str.split(':');
                              if (str.length == 2) {
                                  return new Date().setHours(str[0], str[1]);
                              } else if (str.length == 3) {
                                  return new Date().setHours(str[0], str[1], str[2]);
                              }
                          } else {
                              return "";
                          }
                      },
                      /*convertir strinf en una fecha (date)*/
                      stringToDate: function (strDate) {
                          if (strDate != null) {
                              var d = new Date(strDate);
                              return d;
                          }
                          else {
                              return "";
                          }
                      },
                      stringToTime: function (time) {
                          if (time != null) {
                              var t = time.split(":");
                              return new Date(1999, 0, 1, t[0], t[1]);
                          }
                          else {
                              return "";
                          }
                      },
                      stringToDatev2: function (date) {
                          if (date != null) {
                              var d = date.split("-");
                              return new Date(d[2], d[1], d[0], 0, 0);
                          }
                          else {
                              return "";
                          }
                      },
                      /*Convertir un Date() en un String (date, formato)*/
                      dateToString: function (date, fmt) {
                          if (date != null && fmt != null) {
                              return $filter('date')(new Date(date), fmt);
                          }
                          else {
                              return "";
                          }
                      },
                      /**/
                      comparetohour: function (horaInicio, horaFin) {
                          var hora_inicio = parseInt(horaInicio.split(":")[0]);
                          var minuto_inicio = parseInt(horaInicio.split(":")[1]);
                          var hora_fin = parseInt(horaFin.split(":")[0]);
                          var minuto_fin = parseInt(horaFin.split(":")[1]);
                          if (hora_inicio < hora_fin) {
                              return 1;
                          }
                          else if (hora_inicio == hora_fin) {
                              if (minuto_inicio < minuto_fin) {
                                  return 2;
                              }
                              else if (minuto_inicio > minuto_fin) {
                                  return 3;
                              }
                          }
                          else if (hora_inicio > hora_fin) {
                              return 4;
                          }

                      },
                      getDias: function (mili_ini, mili_fin) {
                          var edad;
                          var fechaActula = mili_ini;
                          var fechaSel = mili_fin;
                          var rs = mili_fin - mili_ini;
                          console.log(rs);
                          var rsr = ((((((rs / 1000) / 60) / 60) / 24) / 30.6) / 12);
                          var strEdad = "";
                          console.log(rsr);
                          if (rsr == 1) {
                              edad += " Año"
                          } else {
                              strEdad = " Años";
                          }
                          if (rsr < 1) {
                              console.log('entro Mes');
                              rsr = rsr * 12;
                              console.log(rsr);
                              if (rsr == 1) {

                                  strEdad = " Mes"
                              } else {
                                  strEdad = " Meses";
                              }
                              if (rsr < 1) {
                                  rsr = rsr * 30;
                                  if (rsr == 1) {
                                      strEdad = " Dia"
                                  } else {
                                      strEdad = " Dias";
                                  }
                              }
                          }

                          return Math.floor(rsr) + " " + strEdad;
                      },
                      getDias2: function (mili_ini, mili_fin) {
                          var edad;
                          var fechaActula = mili_ini;
                          var fechaSel = mili_fin;
                          var rs = mili_fin - mili_ini;
                          var rsr = ((((((rs / 1000) / 60) / 60) / 24) / 30) / 12);
                          var strEdad = "";
                          if (rsr < 1) {
                              rsr = rsr * 30 * 12;
                              if (rsr == 1) {
                                  strEdad = " Dia"
                              } else {
                                  strEdad = " Dias";
                              }
                          }


                          return Math.floor(rsr) + " " + strEdad;
                      },
                      getDiasCompleto: function (mili_ini, mili_fin) {
                          var edad;
                          var fechaActula = mili_ini;
                          var fechaSel = mili_fin;
                          var rs = mili_fin - mili_ini;
                          var rsr = ((((((rs / 1000) / 60) / 60) / 24) / 30.6) / 12);
                          var strEdad = "";
                          if (rsr == 1) {
                              edad += " Año"
                          } else {
                              strEdad = " Años";
                          }
                          if (rsr < 1) {
                              rsr = rsr * 12;
                              if (rsr == 1) {
                                  strEdad = " Mes"
                              } else {
                                  strEdad = " Meses";
                              }
                              if (rsr < 1) {
                                  rsr = rsr * 30;
                                  if (rsr == 1) {
                                      strEdad = " Dia"
                                  } else {
                                      strEdad = " Dias";
                                  }
                              }
                          }

                          return Math.floor(rsr) + " " + strEdad;
                      },
                      sumarDias: function (fecha, tiempo) {
                          console.log("entro sumar dias")
                           var newfecha=fecha,
                               dia = newfecha.getDate(),
                               mes = newfecha.getMonth() + 1,
                               anio = newfecha.getFullYear(),
                             addTime = tiempo * 86400; //Tiempo en segundos

                           newfecha.setSeconds(addTime); //Añado el tiempo
                           return "" + newfecha.getFullYear() + "-" + (newfecha.getMonth() + 1) + "-" + newfecha.getDate()+"";
                      },
                      sumarDias2: function (fecha, dias) {
                          fecha.setDate(fecha.getDate() + dias);
                          return fecha;
                      }

                  };
              };
        function fileread() {
                  return {
                      scope: {
                          fileread: "="
                      },
                      link: function (scope, element, attributes) {
                          element.bind("change", function (changeEvent) {
                              var reader = new FileReader();
                              reader.onload = function (loadEvent) {
                                  scope.$apply(function () { scope.fileread = loadEvent.target.result; });
                              }
                              reader.readAsDataURL(changeEvent.target.files[0]);
                          });
                      }
                  }
              };
        function Service($http) {
                  //var url = 'http://service.inversionespp.com/Service';
                 var url = 'http://sistemasoptecom.net/servicio/Service/';
                //var url = 'http://localhost:2653/Service';
                  //var url = 'http://10.1.36.1:8089/Service';
                  //var url = 'http://127.0.0.1:8089/Service';
                  //var url = 'http://190.182.46.253:8089/Service';

                  var rs = {};
                  rs.getCrud = function (data, crud) {
                      return $http({
                          header: {
                              'content-type': 'application/json;charset=utf-8',
                             
                          },
                          url: url + crud,
                          method: "POST",
                          data: { json: angular.toJson(data) }
                      });
                  }
                  return rs;
              }


})();
