/////////////////////////////////////////////////////////////////////////////////
// @desc 模拟Mono接口
// @copyright ©2018 iGG
// @release 2018年3月30日 星期五
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

namespace Skyunion
{
    public interface IMonoLike
    {
        void Start();
        void Update();
        void OnEnable();
        void OnDisable();

        void LateUpdate();

        void FixedUpdate();
        void OnDestroy();
    }
}
