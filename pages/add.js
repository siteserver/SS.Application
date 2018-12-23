var $url = '/pages/add';

var data = {
  apiUrl: utils.getQueryString('apiUrl'),
  siteId: utils.getQueryString('siteId'),
  returnUrl: utils.getQueryString('returnUrl'),
  pageConfig: null,
  pageLoad: false,
  pageAlert: null,
  dataInfo: null,
  departmentInfoList: null,
  settings: null,
  provideType: [],
  obtainType: []
};

var methods = {
  load: function () {
    var $this = this;

    $api.get($url + '?siteId=' + this.siteId).then(function (response) {
      var res = response.data;

      $this.dataInfo = res.value;
      $this.departmentInfoList = res.departmentInfoList;
      $this.settings = res.settings;
      $this.provideType = [];
      $this.obtainType = [];
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  submit: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url + '?siteId=' + this.siteId, _.assign({}, this.dataInfo, {
      provideType: $this.provideType,
      obtainType: $this.obtainType
    })).then(function (response) {
      swal({
        toast: true,
        type: 'success',
        title: "申请提交成功",
        showConfirmButton: false,
        timer: 2000
      }).then(function () {
        location.href = 'contents.html?siteId=' + $this.siteId + '&state=New&apiUrl=' + encodeURIComponent($this.apiUrl);
      });
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.submit();
      }
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.load();
  }
});