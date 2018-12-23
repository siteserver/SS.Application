var $url = '/templates';

var data = {
  siteId: utils.getQueryString('siteId'),
  apiUrl: utils.getQueryString('apiUrl'),
  pageConfig: null,
  pageAlert: null,
  pageType: 'loading',
  templateType: 'submit',
  templates: null,
  templateHtml: null,
};

var methods = {
  loadTemplates: function (templateType) {
    var $this = this;
    this.templateType = templateType;

    if (this.pageLoad) {
      utils.loading(true);
    }

    $api.get($url + '?siteId=' + this.siteId + '&templateType=' + this.templateType).then(function (response) {
      var res = response.data;

      utils.loading(false);
      $this.templates = res.value;
      $this.pageType = 'list';
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  getPreviewUrl: function (template) {
    return template.templateUrl + '/' + template.id + '/index.html?siteId=' + this.siteId + '&apiUrl=' + encodeURIComponent(this.apiUrl);
  },

  btnGetTemplateDefaultClick: function (template) {
    this.templateHtml = '<stl:application theme="' + template.id + '"></stl:application>';
    this.pageType = 'templateHtml';
    setTimeout(function () {
      $('.js-copytextarea').css({
        height: 65
      });
    }, 100);
  },

  btnGetTemplateHtmlClick: function (template) {
    this.templateHtml = '<stl:application>' + template.html + '</stl:application>';
    this.pageType = 'templateHtml';
    setTimeout(function () {
      $('.js-copytextarea').css({
        height: $(document).height() - 150
      });
    }, 100);
  },

  btnCopyClick: function () {
    var copyTextarea = document.querySelector('.js-copytextarea');
    copyTextarea.focus();
    copyTextarea.select();

    try {
      document.execCommand('copy');
      swal({
        toast: true,
        type: 'success',
        title: "复制成功！",
        showConfirmButton: false,
        timer: 2000
      })
    } catch (err) {}
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.loadTemplates('submit');
  }
});