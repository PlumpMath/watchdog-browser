﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WatchdogBrowser.CustomEventArgs;
using WatchdogBrowser.Models;

namespace WatchdogBrowser.Config {
    public class SitesConfig {

        public event EventHandler<ConfigReadyEventArgs> Ready;

        public List<SiteModel> Sites {
            get {
                return sites;
            }
        }

        List<SiteModel> sites = new List<SiteModel>();

        string configPath;

        public SitesConfig() {
            configPath = Properties.Settings.Default.ConfigFilename;
        }


        public void Initialize() {
            var configText = ReadConfig();
            sites = ParseConfig(configText);
            Ready?.Invoke(this, new ConfigReadyEventArgs(sites));
        }

        string ReadConfig() {
            if (!File.Exists(configPath)) {
                return string.Empty;
            }
            using (var reader = File.OpenText(configPath)) {
                return reader.ReadToEnd();
            }
        }


        List<SiteModel> ParseConfig(string xmlString) {
            List<SiteModel> retval = new List<SiteModel>();
            if (xmlString.Equals(string.Empty)) {
                var siteModel = new SiteModel {
                    Name = "Файл конфигупации не найден",
                    UpdateInterval = 0,
                    SwitchMirrorTimeout = 0,
                    UpdateTimeout = 0,
                    Mirrors = new List<string> { $"http://yandex.ru" },
                    Whitelist = new List<string>()
                };

                retval.Add(siteModel);
                return retval;
            }

            var xdoc = XDocument.Parse(xmlString);
            var xSites = xdoc.Descendants("sites");
            if (xSites.Count() > 0) {
                //берём только первый элемент, по ТЗ он один, но такой код даёт задел на доработку
                var site = xSites.First();
                var siteName = string.Empty;
                int updateOk, updateFail, updateTimeout;
                List<string> mirrors = new List<string>();
                List<string> whitelist = new List<string>();



                try {
                    siteName = site.Attribute("name").Value;
                } catch {
                    siteName = "Неизвестно";
                }

                try {
                    updateOk = int.Parse(site.Attribute("updateOk").Value);
                } catch {
                    updateOk = 10;
                }

                try {
                    updateFail = int.Parse(site.Attribute("updateFail").Value);
                } catch {
                    updateFail = 60;
                }

                try {
                    updateTimeout = int.Parse(site.Attribute("updateTimeout").Value);
                } catch {
                    updateTimeout = 20;
                }

                var xMirrors = site.Descendants("mirrors");
                foreach (var xMirror in xMirrors) {
                    var protocol = string.Empty;
                    var domain = string.Empty;
                    try {
                        protocol = xMirror.Attribute("protocol").Value;
                    } catch {
                        protocol = "http";
                    }

                    try {
                        domain = xMirror.Attribute("domain").Value;
                    } catch {
                        domain = "github.com";
                    }

                    mirrors.Add($"{protocol}://{domain}/");
                }//end foreach xMirrors

                var xWhitelist = site.Descendants("whitelist");
                foreach (var xPath in xWhitelist) {
                    var path = string.Empty;
                    try {
                        path = xPath.Attribute("uri").Value;
                    } catch { }
                    whitelist.Add(path);
                }//end foreach whitelist

                var siteModel = new SiteModel {
                    Name = siteName,
                    UpdateInterval = updateOk,
                    SwitchMirrorTimeout = updateFail,
                    UpdateTimeout = updateTimeout,
                    Mirrors = mirrors,
                    Whitelist = whitelist
                };

                retval.Add(siteModel);

            }//end if count > 0
            return retval;
        }//end PardeConfig


        
    }

}
/*
 * Пример xml
 * <?xml version="1.1" encoding="UTF-8" ?>
 * <sites>
 *  <site name="Кобра Гарант" updateOk="10" updateFail="60" updateTimeout="20">
 *      <mirrors>
 *          <mirror domain="ex1.example.com" protocol="https"/>
 *          <mirror domain="ex2.example.com" protocol="https"/>
 *      </mirrors>
 *      <whitelist>
 *          <path uri="index.php?r=site%2Findex"/>
 *          <path uri="index.php"/>
 *          <path uri="index.php?r=site%2Fobject-list"/>
 *      </whitelist>
 *  </site>
 * </sites>
 */