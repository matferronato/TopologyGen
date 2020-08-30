RED='\033[0;31m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

source ./create_files.sh --source-only --justAWindowsError
source ./lock_files.sh --source-only --justAWindowsError
source ./parse_args.sh --source-only --justAWindowsError
source ./create_dotfile.sh --source-only --justAWindowsError
source ./interface_up.sh --source-only --justAWindowsError
source ./add_server_default_gateway.sh --source-only --justAWindowsError
source ./config_switchs.sh --source-only --justAWindowsError
source ./config_vlan.sh --source-only --justAWindowsError
source ./config_routers.sh --source-only --justAWindowsError
source ./config_bgp.sh --source-only --justAWindowsError
source ./config_vxlan.sh --source-only --justAWindowsError
source ./add_hostnames.sh --source-only --justAWindowsError
source ./create_vagrantfile.sh --source-only --justAWindowsError
source ./running_vagrant.sh --source-only --justAWindowsError
source ./apply_configs.sh --source-only --justAWindowsError
source ./unlock_files.sh --source-only --justAWindowsError
source ../../Automate/User_Scripts/user_script.sh --source-only --justAWindowsError

echo -e ${YELLOW}Running ${BLUE}converter.sh${NC}
